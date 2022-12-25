namespace Hainz.Helpers.Discord;

public static class DiscordText 
{
    public static string WrapInCode(string message, string? code = "") =>
        $"```{code}\n{message}\n```";
}