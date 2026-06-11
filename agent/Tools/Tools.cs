using agent;
using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
namespace agent.Tools;
public static class Tools
{
    public static List<AITool> GetAll()
    {
        return new List<AITool>
        {
            AIFunctionFactory.Create(GetWordReversed),
            AIFunctionFactory.Create(GetBestPlayer),
            AIFunctionFactory.Create(CalculateExpression),
            AIFunctionFactory.Create(RunReasoningAgent)
        };
    }

    [Description("Reverses a word")]
    public static string GetWordReversed(string word)
    {
        Console.WriteLine("TOOL EXECUTED");

        return new string(word.Reverse().ToArray());
    }

    [Description("Gets best player")]
    public static string GetBestPlayer()
        => "Hameedooo";

    [Description("Evaluate a math expression like 2+2*5")]
    public static string CalculateExpression(string expression)
    {
        try
        {
            var result = new DataTable().Compute(expression, null);
            return $"Result = {result}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [Description("Reasoning Agent")]
    public static async Task<string> RunReasoningAgent(string question)
    {
        var client = new OpenAIClient(
            new ApiKeyCredential(AppConfig.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(AppConfig.Endpoint)
            });

        var chatClient = client
            .GetChatClient("gpt-5-mini")
            .AsIChatClient();

        var agent = chatClient.AsAIAgent(
            name: "ReasoningAgent",
            instructions: "You are a deep reasoning assistant. Explain step by step.");

        var session = await agent.CreateSessionAsync();

        var response = await agent.RunAsync(question, session);

        return response.Text;
    }
}