using agent.Logging;
using agent.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;


namespace agent.techstore
{
    public static class ProductAgentFactory
    {
        public static AIAgent Create(AiModel model)
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

            var allTools = new List<AITool>();

            allTools.AddRange(ToolRegistry.GetAll());

            // Main Agent
            var options = new ChatClientAgentOptions
            {
                ChatOptions = new ChatOptions
                {

                    ToolMode = ChatToolMode.Auto,
                    Tools = allTools,
                    Instructions =
"""
You are a website chatbot.

You MUST:
- Always use tools before answering
- Never hallucinate data
- Only use website content
- Return structured JSON response

If user asks:
- products → use GetProducts
- pages → use GetPages
- general → combine tools
"""
                }
            };

            var agent = chatClient
                .AsAIAgent(options)
                .AsBuilder()
               
                .Build();

            return agent;
        }
    }
}
