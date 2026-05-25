namespace ConsoleApp1;

public enum AiModel
{
    Gemma3_1b,
    Gemini31FlashLite
}

public static class ModelCatalog
{
    public static string ToModelName(AiModel model)
    {
        return model switch
        {
            AiModel.Gemma3_1b => "google/gemma-3-1b",
            AiModel.Gemini31FlashLite => "gemini-3.1-flash-lite",
            _ => throw new ArgumentOutOfRangeException(nameof(model), model, "Unknown model")
        };
    }
}
