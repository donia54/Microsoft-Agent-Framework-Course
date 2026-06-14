using agent.techstore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.techstore.Data
{
    public static class MockDatabase
    {
        public static List<Page> Pages = new()
    {
        new Page
        {
            Url = "/phones",
            Content = "We sell iPhone 16 and Galaxy S25"
        },
        new Page
        {
            Url = "/laptops",
            Content = "MacBook Air M4 available"
        }
    };

        public static List<Product> Products = new()
    {
        new() { Name="iPhone 16", Price=1200, InStock=true },
        new() { Name="Galaxy S25", Price=1000, InStock=true },
        new() { Name="MacBook Air M4", Price=1500, InStock=false }
    };
    }
}
