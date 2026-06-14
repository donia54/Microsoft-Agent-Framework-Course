using agent.Logging;
using agent.techstore.RAG;
using agent.Tools;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.SqliteVec;
using OpenAI;
using System.ClientModel;
using System.ClientModel.Primitives;


namespace agent.techstore
{
    public static class ProductAgentFactory
    {
        public  static AIAgent Create(AiModel model)
        {

            var handler = new CustomClientHttpHandler();
            var httpClient = new HttpClient(handler);
            var client = new OpenAIClient(
            new ApiKeyCredential(AppConfig.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(AppConfig.Endpoint),
                Transport = new HttpClientPipelineTransport(httpClient)
            });

            var chatClient = client
                .GetChatClient(ModelCatalog.ToModelName(model))
                .AsIChatClient();


            var embeddingGenerator =
                client.GetEmbeddingClient("text-embedding-3-small")
                      .AsIEmbeddingGenerator();

            string dbPath = $"Data Source={Path.GetTempPath()}\\techstore.db";

            var vectorStore =
                new SqliteVectorStore(dbPath,
                new SqliteVectorStoreOptions
                {
                    EmbeddingGenerator = embeddingGenerator
                });

            // Services
            var storeService = new VectorStoreService(vectorStore);
             storeService.EnsureAsync();

            var ingest = new RAGIngestionService(storeService);

            // أول مرة بس
             storeService.ResetAsync();
             ingest.IngestAsync();



            var allTools = new List<AITool>();

            allTools.AddRange(ToolRegistry.GetAll(storeService));


            // Main Agent
            var options = new ChatClientAgentOptions
            {
                ChatOptions = new ChatOptions
                {

                    ToolMode = ChatToolMode.Auto,
                    Tools = allTools,
                    Instructions =
"""
You are a website assistant.
ALWAYS use RAG tool first before other tools.
If query is about products, search RAG first.
Only use raw tools if RAG returns nothing.

Available tools:

SearchWebsite(query)
GetProducts()

Rules:
- Never invent products.
- Always call a tool first.
- Answer only from returned data.
- Include source urls.
"""
                }
            };

          


            var agent = chatClient
                .AsAIAgent(options)
                .AsBuilder()
               
                .Build();

            return agent;
        }
    }
}
