
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

var openAiClient = new OpenAIClient(
    new ApiKeyCredential("lm-studio"),
    new OpenAIClientOptions
    {
        Endpoint = new Uri("http://localhost:1234/v1")
    });

IChatClient client = openAiClient
    .GetChatClient("ibm/granite-4-micro")
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
