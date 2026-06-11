using GenerativeAI.Types;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Containers;
using OpenAI.Responses;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.Agents
{
    public static class CodeInterpreterAgentFactory
    {
        public static AIAgent Create(OpenAIClient client, AiModel model)
        {
#pragma warning disable OPENAI001

            var responsesClient = client.GetResponsesClient();

            var chatClient = responsesClient.AsIChatClient(ModelCatalog.ToModelName(model));

            return chatClient.AsAIAgent(
                name: "CodeInterpreterAgent",
                instructions:
                    "Use code interpreter to solve math problems and create charts.",
                tools:
                [
                    new HostedCodeInterpreterTool()
                ]);

#pragma warning restore OPENAI001
        }
    }
}