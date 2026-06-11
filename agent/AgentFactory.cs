using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using agent.Agents;
using System.ClientModel;
using agent.Logging;
using agent.Tools;

namespace agent;

public static class AgentFactory
{
    public static async Task<AIAgent> CreateAsync(AiModel model)
    {
        var client = new OpenAIClient(
            new ApiKeyCredential(AppConfig.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(AppConfig.Endpoint)
            });

        var chatClient = client
            .GetChatClient(ModelCatalog.ToModelName(model))
            .AsIChatClient();

        var mcpTools = await McpService.LoadToolsAsync();

        var allTools = new List<AITool>();

        // local tools
        allTools.AddRange(Tools.Tools.GetAll());
       

        // MCP tools
        allTools.AddRange(mcpTools);

        // sub agents
        var calculatorAgent = MathAgentFactory.Create(chatClient);
        var reasoningAgent = ReasoningAgentFactory.Create(chatClient);
        //interpreter agent tool 
        var codeInterpreterAgent = CodeInterpreterAgentFactory.Create(client,model);

        // Convert agents to tools
        var agentTools = new List<AITool>
        {
            calculatorAgent.AsAIFunction(),
            reasoningAgent.AsAIFunction(),
            codeInterpreterAgent.AsAIFunction()
        };

        allTools.AddRange(agentTools);

        // Main Agent
        var options = new ChatClientAgentOptions
        {
            ChatOptions = new ChatOptions
            {
               
                ToolMode = ChatToolMode.Auto,
                Tools = allTools
            }
        };

        var agent = chatClient
            .AsAIAgent(options)
            .AsBuilder()
            .Use(ToolLoggingMiddleware.InvokeAsync)
           // .Use(AgentTracingMiddleware.InvokeAsync)
            .Build();

        return agent;
    }
}