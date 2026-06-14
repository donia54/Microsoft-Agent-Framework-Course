using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agent.techstore.RAG
{
    public class EmbeddingService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;

        public EmbeddingService(IEmbeddingGenerator<string, Embedding<float>> generator)
        {
            _generator = generator;
        }

        public async Task<Embedding<float>> EmbedAsync(string text)
        {
            return await _generator.GenerateAsync(text);
        }
    }
}
