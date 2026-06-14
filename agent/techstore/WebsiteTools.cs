using Microsoft.Extensions.AI;
using System.ComponentModel;
using agent.techstore.Data;
using agent.techstore.Models;
using Microsoft.SemanticKernel.Services;
using agent.techstore.RAG;

namespace agent.Tools;

public static class WebsiteTools
{



    [Description("Search website pages")]
    public static string SearchWebsite(string query)
    {
        var results = MockDatabase.Pages
            .Where(p =>
                p.Content.Contains(query,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!results.Any())
            return "No pages found";

        return string.Join("\n\n",
            results.Select(x =>
                $"URL: {x.Url}\nContent: {x.Content}"));
    }

    [Description("Returns all pages")]
    public static List<Page> GetPages()
    {
        return MockDatabase.Pages;
    }

    [Description("Returns all products in the store")]
    public static List<Product> GetProducts()
    {
        return MockDatabase.Products;
    }

}