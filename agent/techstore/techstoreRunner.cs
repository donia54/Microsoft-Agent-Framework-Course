using agent.techstore.RAG;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Connectors.SqliteVec;
using OpenAI;
using System;
using System.ClientModel;
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

            // Embedding
            var embeddingGenerator =
                client.GetEmbeddingClient("text-embedding-3-small")
                      .AsIEmbeddingGenerator();

            // Chat model (GPT)
            var chatClient =
                client.GetChatClient("gpt-4.1-nano")
                      .AsIChatClient();

            // Vector store
            string dbPath = $"Data Source={Path.GetTempPath()}\\techstore.db";

            var vectorStore =
                new SqliteVectorStore(dbPath,
                new SqliteVectorStoreOptions
                {
                    EmbeddingGenerator = embeddingGenerator
                });

            // Services
            var storeService = new VectorStoreService(vectorStore);
            var ingest = new RAGIngestionService(storeService);

            // 🔥 RAG SERVICE
            var rag = new RAGService(storeService, chatClient);

            // init
            await storeService.ResetAsync();
            await ingest.IngestAsync();



            while (true)
            {
                Console.Write("\nAsk: ");
                string question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question))
                    continue;

                Console.WriteLine("\n🤖 Thinking...\n");

                string answer = await rag.AskAsync(question);

                Console.WriteLine("\n====================");
                Console.WriteLine("FINAL ANSWER:");
                Console.WriteLine("====================");
                Console.WriteLine(answer);
            }
        }
    }
}
