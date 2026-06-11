using Microsoft.Extensions.AI;
using agent.techstore;
using System.ComponentModel;

namespace agent.Tools;

public static class WebsiteTools
{
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