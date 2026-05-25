using GenerativeAI.Types;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using OpenAI;
using System.ClientModel;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace ConsoleApp1;

public class Local
{

    static string endpoint = "http://localhost:1234/v1";
    static string apiKey = "lkm-studio";
    static string UserDefaultMessage = "Reverse the word Latinooo using the reverse_word function only";


    static public async Task RunLocalAsync(AiModel model)
    {
        var openAiClient = new OpenAIClient(new ApiKeyCredential(apiKey),new OpenAIClientOptions{Endpoint = new Uri(endpoint)});

        IChatClient client = openAiClient.GetChatClient(ModelCatalog.ToModelName(model)).AsIChatClient();

        ChatClientAgent agent = new(client);

        while (true)
        {
            Console.Write("User: ");
            var userInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(userInput))
                break;

            var response = await agent.RunAsync(userInput);

            Console.WriteLine("Agent: " + response.Text);
        }
    }

    // With agent
    static public async Task RunLocalWithAgentAsync(AiModel model, bool streaming = true)
    {

        await using McpClient mcpClient = await McpClient.CreateAsync(new HttpClientTransport(new HttpClientTransportOptions
        {
            Endpoint = new Uri("https://learn.microsoft.com/api/mcp"),
            TransportMode = HttpTransportMode.StreamableHttp
        }));
        IList<McpClientTool> mcpTools = await mcpClient.ListToolsAsync();


        OpenAIClient client = new OpenAIClient(new ApiKeyCredential(apiKey), new OpenAIClientOptions { Endpoint = new Uri(endpoint) });

        var chatClient = client.GetChatClient(ModelCatalog.ToModelName(model)).AsIChatClient();
     
        var allTools = getAllFunctions();                     
        allTools.AddRange(mcpTools.Cast<AITool>());           


        var agentOptions = new ChatClientAgentOptions
        {
            ChatOptions = new ChatOptions
            {
                ToolMode = ChatToolMode.Auto,
                Tools = allTools,
                Reasoning = new ReasoningOptions
                {
                    Effort = ReasoningEffort.ExtraHigh
                }
            }
        };

        AIAgent agent = chatClient.AsAIAgent(agentOptions);
        
        AgentSession session = await agent.CreateSessionAsync();

        Console.Write("User: ");
        var userInput = Console.ReadLine();
        Console.Write("Agent: ");

        if (streaming)
        {

            await foreach (var response in agent.RunStreamingAsync(userInput, session))
            {
                Console.Write(response.Text);
            }
            Console.WriteLine(); // for new line after streaming is done
        }
        else
        {
            AgentResponse response = await agent.RunAsync(userInput, session);

            Console.WriteLine(response.FinishReason);

            Console.WriteLine(response.Text);
        }        
    }

    static List<AITool> getAllFunctions()
    {
        var reverseWordTool = AIFunctionFactory.Create(GetWordReversed);
        var bestPlayerTool = AIFunctionFactory.Create(GetBestPlayer);

        return new List<AITool>() { reverseWordTool, bestPlayerTool };
        
    }


    [Description("Reverses a word")]
    static string GetWordReversed(string word)
    {
        return new string(word.Reverse().ToArray())  + "1";
    }

    [Description("Gets the name of the best player in the world")]
    static string GetBestPlayer()
    {
        return "Hameedooo";
    }
}