using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.Agents
{
    public static class ReasoningAgentFactory
    {
        public static AIAgent Create(IChatClient client)
        {
            return client
                .AsAIAgent(
                    name: "ReasoningAgent",
                    instructions: "YALWAYS use this tool for any reasoning, math explanation, or step-by-step thinking");
        }
    }
}
