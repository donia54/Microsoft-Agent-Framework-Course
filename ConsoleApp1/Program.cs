using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using GenerativeAI.Microsoft;

IChatClient client = 
    new GenerativeAIChatClient(apiKey: "AIzaSyBESIweduXJzRBT8tk5If0nyf0TCw_Wpjo", "gemini-3.1-flash-lite");

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
