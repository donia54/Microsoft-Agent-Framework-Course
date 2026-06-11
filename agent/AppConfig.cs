namespace agent;

public static class AppConfig
{
    public static string Endpoint => "https://api.openai.com/v1";
    public static string ApiKey =>
        Environment.GetEnvironmentVariable("OPENAI_API_KEY")
        ?? throw new Exception("OPENAI_API_KEY not found");
}