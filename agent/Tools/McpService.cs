using ModelContextProtocol.Client;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace agent.Tools;
public static class McpService
{
    public static async Task<List<McpClientTool>> LoadToolsAsync()
    {
        await using var client = await McpClient.CreateAsync(
            new HttpClientTransport(new HttpClientTransportOptions
            {
                Endpoint = new Uri("https://learn.microsoft.com/api/mcp"),
                TransportMode = HttpTransportMode.StreamableHttp
            }));
      

        return (await client.ListToolsAsync()).ToList();
    }
}