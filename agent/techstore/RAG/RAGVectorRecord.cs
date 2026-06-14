using Microsoft.Extensions.VectorData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.techstore.RAG
{
    public class RAGVectorRecord
    {
        [VectorStoreKey]
        public Guid Id { get; set; }

        [VectorStoreData]
        public string Content { get; set; } = "";

        [VectorStoreData]
        public string Type { get; set; } = "";

        [VectorStoreVector(1536)]
        public string Vector => Content;
    }
}
