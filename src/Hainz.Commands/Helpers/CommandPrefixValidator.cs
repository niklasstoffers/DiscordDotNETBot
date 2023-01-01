namespace Hainz.Commands.Helpers;

public static class CommandPrefixValidator
{
    public static readonly char[] ValidPrefixes = 
    { 
        '!', 
        '.', 
        '$', 
        '^',
        '&',
        '%'
    };

    public static bool IsValidPrefix(char prefix) => ValidPrefixes.Contains(prefix);
}