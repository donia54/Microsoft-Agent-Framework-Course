using Microsoft.Agents.AI;
using OpenAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.techstore
{
    class techstoreRunner
    {
        public static async Task RunStreamingAsync(AiModel model, OpenAIClient client)
        {
            var agent =  ProductAgentFactory.Create(model);
            var session = await agent.CreateSessionAsync();




            while (true)
            {
                Console.Write("User: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                Console.Write("Agent: ");

                AgentResponse <ChatResponse> response = await agent.RunAsync<ChatResponse>(input, session);

                var result = response.Result;
                Console.WriteLine("\n--- RESPONSE ---");
                Console.WriteLine(response.Result.Answer);

                Console.WriteLine("\nSources:");
                response.Result.Sources.ForEach(Console.WriteLine);

                Console.WriteLine($"\nIntent: {response.Result.Intent}");
                Console.WriteLine();


            }
        }
    }
}
