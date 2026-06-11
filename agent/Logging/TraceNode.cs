using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.Logging
{
    public class TraceNode
    {
        public string Name { get; set; }
        public string Type { get; set; } // Main / Tool / SubAgent
        public string Input { get; set; }
        public string Output { get; set; }
        public List<TraceNode> Children { get; set; } = new();
    }
}
