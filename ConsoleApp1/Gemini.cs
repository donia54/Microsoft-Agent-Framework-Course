using GenerativeAI.Microsoft;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1;

public class Gemini
{
    static public async Task RunGeminiModel(AiModel model)
    {
        IChatClient client =
    new GenerativeAIChatClient(apiKey: "AIzaSyBESIweduXJzRBT8tk5If0nyf0TCw_Wpjo", ModelCatalog.ToModelName(model));

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
