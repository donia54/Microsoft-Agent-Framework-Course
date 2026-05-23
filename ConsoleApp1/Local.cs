using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace ConsoleApp1;

public class Local
{
    static public async Task RunLocalAsync(string model)
    {
        var openAiClient = new OpenAIClient(
            new ApiKeyCredential("lm-studio"),
            new OpenAIClientOptions
            {
                Endpoint = new Uri("http://localhost:1234/v1")
            });

        IChatClient client = openAiClient
            .GetChatClient(model)
            .AsIChatClient();

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
}