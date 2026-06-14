using Microsoft.Extensions.AI;
using Microsoft.Extensions.VectorData;
using OpenAI;
using System.ClientModel;
using Microsoft.SemanticKernel.Connectors.SqliteVec;
using System.Text;

namespace agent
{
    class RAGTesting
    {
        public static async Task RunSample()
        {
            // ======================
            // 1. Knowledge Base Data
            // ======================
            List<KnowledgeBaseEntry> knowledgeBase =
            [
                new("What is the WI-FI Password at the Office?", "The Password is 'Guest42'"),
                new("Is Christmas Eve a full or half day off", "It is a full day off"),
                new("How do I register vacation?", "Go to the internal portal and ..."),
                new("What do I need to do if I'm sick?", "Inform your manager"),
                new("Where is the employee handbook?", "It is located here"),
                new("Who is in charge of support?", "John Doe"),
                new("I can't log in to my office account", "Susan can reset your password"),
                new("CRM error index out of bounds", "Log out and log in again"),
                new("Buying books policy", "Under 20$ no approval needed"),
                new("Hiring bounty", "1000$ for successful hire")
            ];

            // ======================
            // 2. OpenAI Client
            // ======================
            var client = new OpenAIClient(
                new ApiKeyCredential(AppConfig.ApiKey),
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(AppConfig.Endpoint)
                });

            IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator =
                client.GetEmbeddingClient("text-embedding-3-small")
                      .AsIEmbeddingGenerator();

            // ======================
            // 3. Vector Store
            // ======================
            string connectionString =
                $"Data Source={Path.GetTempPath()}\\af-course-vector-store.db";

            VectorStore vectorStore =
                new SqliteVectorStore(connectionString,
                new SqliteVectorStoreOptions
                {
                    EmbeddingGenerator = embeddingGenerator
                });

            var collection =
                vectorStore.GetCollection<Guid, KnowledgeBaseVectorRecord>("knowledge_base");

            await collection.EnsureCollectionExistsAsync();

            // ======================
            // 4. Import Data
            // ======================
            Console.Write("Import Data? (Y/N): ");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                await collection.EnsureCollectionDeletedAsync();
                await collection.EnsureCollectionExistsAsync();

                int i = 0;
                foreach (var item in knowledgeBase)
                {
                    i++;
                    Console.Write($"\rEmbedding {i}/{knowledgeBase.Count}");

                    await collection.UpsertAsync(new KnowledgeBaseVectorRecord
                    {
                        Id = Guid.NewGuid(),
                        Question = item.Question,
                        Answer = item.Answer
                    });
                }

                Console.WriteLine("\nDone Embedding!");
            }

            Console.WriteLine("\n=============================");
            Console.WriteLine("🔥 RAG CHAT READY");
            Console.WriteLine("=============================\n");

            // ======================
            // 5. CHAT LOOP (RAG PART)
            // ======================
            while (true)
            {
                Console.Write("\nAsk: ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                    continue;

                // ======================
                // 6. VECTOR SEARCH
                // ======================
                StringBuilder context = new();

                await foreach (var result in collection.SearchAsync(input, 3))
                {
                    context.AppendLine(
                        $"Q: {result.Record.Question}\nA: {result.Record.Answer}\n");
                }

                Console.WriteLine("\n--- Retrieved Context ---");
                Console.WriteLine(context.ToString());

                // ======================
                // 7. LLM RESPONSE (RAG)
                // ======================
                IChatClient chatClient =
          client.GetChatClient("gpt-4.1-nano")
                .AsIChatClient();

                var response = await chatClient.GetResponseAsync(
     $"You are an internal company assistant. Answer ONLY using the provided context.\n\n" +
     $"Context:\n{context}\n\nQuestion: {input}"
 );

                Console.WriteLine(response);
            }
        }

        // ======================
        // Models
        // ======================
        public record KnowledgeBaseEntry(string Question, string Answer);

        public class KnowledgeBaseVectorRecord
        {
            [VectorStoreKey]
            public required Guid Id { get; set; }

            [VectorStoreData]
            public required string Question { get; set; }

            [VectorStoreData]
            public required string Answer { get; set; }

            [VectorStoreVector(1536)]
            public string Vector => $"Q: {Question} - A: {Answer}";
        }
    }
}