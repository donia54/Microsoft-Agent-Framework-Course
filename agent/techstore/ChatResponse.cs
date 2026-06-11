using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.techstore
{
    public class ChatResponse
    {
        public string Answer { get; set; } = "";

        public List<string> Sources { get; set; } = new();

        public string Intent { get; set; } = "";
    }
}
