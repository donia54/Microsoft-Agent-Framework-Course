using System;
using System.Threading.Tasks;
using Microsoft.Agents.AI;
using agent;
using agent.Logging;
using Microsoft.Extensions.AI;
using OpenAI.Containers;
using OpenAI.Responses;
using System.ClientModel;
using OpenAI;
using System.Diagnostics;
using agent.techstore;
public static class Runner
{    public static async Task RunStreamingAsync(AiModel model, OpenAIClient client)
    {
        var agent = await AgentFactory.CreateAsync(model);
        var session = await agent.CreateSessionAsync();
      



        while (true)
        {
            Console.Write("User: ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            Console.Write("Agent: ");

            AgentResponse response = await agent.RunAsync(input, session);
            Console.Write(response.Text);
            // Console.WriteLine(response);
          
           
        }
    }
}