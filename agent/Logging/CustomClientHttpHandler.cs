using Amazon.CloudFormation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace agent.Logging
{
    class CustomClientHttpHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string requestString = await request.Content?.ReadAsStringAsync(cancellationToken)!;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Raw Request ({request.RequestUri})");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(MakePretty(requestString));

            Console.ResetColor();
            Console.WriteLine(new string('-', 50));
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            string responseString = await response.Content.ReadAsStringAsync(cancellationToken);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Raw Response");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(MakePretty(responseString));
            Console.WriteLine(new string('-', 50));
            return response;
        }

        private string MakePretty(string input)
        {
            try
            {
                JsonElement jsonElement = JsonSerializer.Deserialize<JsonElement>(input);
                return JsonSerializer.Serialize(jsonElement, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception e)
            {
                return input;
            }
        }
    }
}
