using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System.Text;

namespace agent.Logging;

public static class AgentTracingMiddleware
{
    public static async ValueTask<object?> InvokeAsync(
        AIAgent agent,
        FunctionInvocationContext context,
        Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next,
        CancellationToken ct)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("\n================ TOOL CALL ================");
        Console.WriteLine($"Agent   : {agent.Name}");
        Console.WriteLine($"Tool    : {context.Function.Name}");

        if (context.Arguments.Count > 0)
        {
            Console.WriteLine("Arguments:");

            foreach (var arg in context.Arguments)
            {
                Console.WriteLine($"  - {arg.Key} = {arg.Value}");
            }
        }

        Console.WriteLine("===========================================\n");

        Console.ResetColor();

        var result = await next(context, ct);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Result: {result}");
        Console.ResetColor();

        Console.WriteLine("\n-------------------------------------------\n");

        return result;
    }
}