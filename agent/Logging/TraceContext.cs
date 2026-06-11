using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.Logging
{
    public static class TraceContext
    {
        public static TraceNode Root = new TraceNode
        {
            Name = "MainAgent",
            Type = "MAIN"
        };

        public static Stack<TraceNode> Stack = new();
    }
}
