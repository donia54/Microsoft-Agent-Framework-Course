using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Connectors.SqliteVec;

namespace agent.techstore.RAG;

public class VectorStoreService
{
    private readonly VectorStoreCollection<Guid, RAGVectorRecord> _collection;

    public VectorStoreService(VectorStore store)
    {
        _collection =
            store.GetCollection<Guid, RAGVectorRecord>("techstore_rag");
    }

    public async Task EnsureAsync()
    {
        await _collection.EnsureCollectionExistsAsync();
    }

    public async Task AddAsync(string content, string type)
    {
        await _collection.UpsertAsync(new RAGVectorRecord
        {
            Id = Guid.NewGuid(),
            Content = content,
            Type = type
        });
    }

    public IAsyncEnumerable<VectorSearchResult<RAGVectorRecord>>
        SearchAsync(string query, int topK = 3)
    {
        Console.WriteLine("\n====================");
        Console.WriteLine("🔎 VECTOR SEARCH START");
        Console.WriteLine($"Query: {query}");
        Console.WriteLine("====================\n");

        return _collection.SearchAsync(query, topK);

    }

    public async Task ResetAsync()
    {
        await _collection.EnsureCollectionDeletedAsync();
        await _collection.EnsureCollectionExistsAsync();
    }
}