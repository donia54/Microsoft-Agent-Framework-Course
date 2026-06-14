using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using GenerativeAI.Microsoft;
using agent;
using OpenAI;
using System.ClientModel;
using agent.techstore;
using agent.techstore.RAG;



//await Local.RunLocalWithAgentAsync(AiModel.Gemma3_1b, streaming: false);
var client = new OpenAIClient(
    new ApiKeyCredential(AppConfig.ApiKey),
    new OpenAIClientOptions
    {
        Endpoint = new Uri(AppConfig.Endpoint)
    });

//await Runner.RunStreamingAsync(AiModel.GPT5Mini,client);

await techstoreRunner.RunStreamingAsync(AiModel.GPT5Mini, client);

//await RAGTesting.RunSample();



Console.ReadLine();
