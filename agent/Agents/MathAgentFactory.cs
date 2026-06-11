using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;


namespace agent.Agents
{
    public static class MathAgentFactory
    {
        public static AIAgent Create(IChatClient client)
        {
            return client
                .AsAIAgent(
                    name: "CalculatorAgent",
                    instructions: "You are a math expert. Solve only math problems clearly and return final result only.");
        }
    }
}
