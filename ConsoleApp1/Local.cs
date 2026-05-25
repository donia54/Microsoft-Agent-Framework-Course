using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace ConsoleApp1;

public class Local
{

    static string endpoint = "http://localhost:1234/v1";
    static string apiKey = "lkm-studio";
    static string defaultUserMessage = "Name the countries in Mama Africa with its cities";
    static string defaultSystemMessage = "You are a helpful assistant that helps users to find out the countries in Africa with its cities";


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
        OpenAIClient client = new OpenAIClient( new ApiKeyCredential(apiKey), new OpenAIClientOptions{Endpoint = new Uri(endpoint)});

        var chatClient = client.GetChatClient(ModelCatalog.ToModelName(model)).AsIChatClient();
        AIAgent agent = chatClient.AsAIAgent(instructions: "You are a Palestinian from Ramallah.");

        AgentSession session = await agent.CreateSessionAsync();



        while (true)
        {
            Console.Write("User: ");
            var userInput = Console.ReadLine();


            ChatMessage systemMessgae = new ChatMessage(ChatRole.System, defaultSystemMessage);
            ChatMessage userMessage = new ChatMessage(ChatRole.User, userInput);

            if (streaming)
            {
                Console.Write("Agent: ");

                await foreach (var response in agent.RunStreamingAsync([systemMessgae, userMessage], session))
                {
                    Console.Write(response.Text);
                }
                Console.WriteLine(); // for new line after streaming is done
            }
            else
            {
                AgentResponse response = await agent.RunAsync(userMessage, session);
                Console.WriteLine("Agent: " + response.Text);
            }
        }
    }
} 