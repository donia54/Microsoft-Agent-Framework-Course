public enum AiModel
{
    GPT5Mini
}

public static class ModelCatalog
{
    public static string ToModelName(AiModel model)
    {
        return model switch
        {
            AiModel.GPT5Mini => "gpt-5-mini",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}