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
        public static List<AITool> GetAll()
        {
            return new()
        {
            AIFunctionFactory.Create(WebsiteTools.GetPages),
            AIFunctionFactory.Create(WebsiteTools.GetProducts)
        };
        }
    }
}
