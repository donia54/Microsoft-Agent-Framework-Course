using agent.techstore.Data;

namespace agent.techstore.RAG;

public class RAGIngestionService
{
    private readonly VectorStoreService _store;

    public RAGIngestionService(VectorStoreService store)
    {
        _store = store;
    }

    public async Task IngestAsync()
    {
        // Products
        foreach (var p in MockDatabase.Products)
        {
            string text =
                $"Product: {p.Name} | Price: {p.Price} | InStock: {p.InStock}";

            await _store.AddAsync(text, "product");
        }

        // Pages
        foreach (var page in MockDatabase.Pages)
        {
            string text =
                $"Page: {page.Url} | Content: {page.Content}";

            await _store.AddAsync(text, "page");
        }
    }
}