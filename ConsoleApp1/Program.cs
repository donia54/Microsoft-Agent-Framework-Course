using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using GenerativeAI.Microsoft;
using ConsoleApp1;



//await Local.RunLocalAsync(AiModel.Gemma3_1b);

await Local.RunLocalWithAgentAsync(AiModel.gpt_oss_20b, streaming: false);

//await Gemini.RunGeminiModel(AiModel.Gemini31FlashLite);


Console.ReadLine();
