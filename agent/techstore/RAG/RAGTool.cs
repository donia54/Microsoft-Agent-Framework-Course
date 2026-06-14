using System.ComponentModel;
using System.Text;

namespace agent.techstore.RAG;

public class RAGTool
{
    private readonly VectorStoreService _store;

    public RAGTool(VectorStoreService store)
    {
        _store = store;
    }

    [Description("Search products and website using semantic search")]
    public async Task<string> Search(string query)
    {
        var results = _store.SearchAsync(query, 3);

        StringBuilder sb = new();

        await foreach (var r in results)
        {
            sb.AppendLine(r.Record.Content);
        }

        return sb.ToString();
    }
}