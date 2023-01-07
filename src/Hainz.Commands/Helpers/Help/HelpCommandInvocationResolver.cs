using Discord;

namespace Hainz.Commands.Helpers.Help;

public class HelpCommandInvocationResolver
{
    private readonly CommandPrefixResolver _prefixResolver;

    public HelpCommandInvocationResolver(CommandPrefixResolver prefixResolver)
    {
        _prefixResolver = prefixResolver;
    }

    public async Task<string> GetSearchInvocation(IChannel channel) =>
        $"{await _prefixResolver.GetPrefix(channel)}help <search>";
}