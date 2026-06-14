using agent.techstore.RAG;
using agent.Tools;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.techstore
{
    public static class ToolRegistry
    {
        public static List<AITool> GetAll(VectorStoreService store)
        {
            var ragTool = new RAGTool(store);

            return new List<AITool>
        {
             AIFunctionFactory.Create(WebsiteTools.SearchWebsite),
            AIFunctionFactory.Create(WebsiteTools.GetProducts),
            AIFunctionFactory.Create(ragTool.Search) 
        };
        }
    }
}
