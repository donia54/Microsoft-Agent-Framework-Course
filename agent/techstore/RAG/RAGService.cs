using Microsoft.Extensions.AI;
using System.Text;
using agent.techstore.RAG;

namespace agent.techstore.RAG;

public class RAGService
{
    private readonly VectorStoreService _store;
    private readonly IChatClient _chatClient;

    public RAGService(
        VectorStoreService store,
        IChatClient chatClient)
    {
        _store = store;
        _chatClient = chatClient;
    }

    public async Task<string> AskAsync(string question)
    {
        Console.WriteLine("\n====================");
        Console.WriteLine("🧠 NEW RAG REQUEST");
        Console.WriteLine($"User Question: {question}");
        Console.WriteLine("====================\n");

        // 1. SEARCH
        Console.WriteLine("🔍 Step 1: Searching Vector DB...\n");

        StringBuilder context = new();
        int i = 1;

        await foreach (var result in _store.SearchAsync(question, 3))
        {
            Console.WriteLine($"[Result {i}] Score: {result.Score}");
            Console.WriteLine(result.Record.Content);
            Console.WriteLine("-------------------");

            context.AppendLine(result.Record.Content);
            i++;
        }

        // 2. CONTEXT
        Console.WriteLine("\n📦 Step 2: Context Built:");
        Console.WriteLine(context.ToString());

        // 3. BUILD PROMPT
        Console.WriteLine("\n🧾 Step 3: Sending to GPT...\n");

        var messages = new List<ChatMessage>
    {
        new ChatMessage(ChatRole.System,
            "You are a TechStore assistant. Use only provided context."),

        new ChatMessage(ChatRole.User,
            $"Context:\n{context}\n\nQuestion:\n{question}")
    };

        Console.WriteLine("📤 GPT INPUT:");
        Console.WriteLine(messages.Last().Text);

        // 4. GPT CALL
        var response = await _chatClient.GetResponseAsync(messages);

        var answer = response.Messages.Last().Text;

        // 5. FINAL RESULT
        Console.WriteLine("\n====================");
        Console.WriteLine("✅ FINAL ANSWER");
        Console.WriteLine("====================");
        Console.WriteLine(answer);

        return answer;
    }
}