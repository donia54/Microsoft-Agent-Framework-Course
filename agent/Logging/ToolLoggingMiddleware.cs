using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.Logging
{
    public static class ToolLoggingMiddleware
    {
        public static async ValueTask<object?> InvokeAsync(
            AIAgent agent,
            FunctionInvocationContext context,
            Func<FunctionInvocationContext, CancellationToken, ValueTask<object?>> next,
            CancellationToken cancellationToken)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("====================================");
            Console.WriteLine($"AGENT: {agent.Name}");
            Console.WriteLine($"TOOL: {context.Function.Name}");

            if (context.Arguments.Count > 0)
            {
                Console.WriteLine("ARGS:");
                foreach (var arg in context.Arguments)
                {
                    Console.WriteLine($"   - {arg.Key}: {arg.Value}");
                }
            }

            Console.WriteLine("====================================");

            Console.ResetColor();

            var result = await next(context, cancellationToken);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"RESULT: {result}");
            Console.ResetColor();

            return result;
        }
    }
}
