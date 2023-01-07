using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules.Admin.Parameters;

[NamedArgumentType]
public sealed class BanOptionsParameter
{
    [NamedCommandParameter("reason", "reason why user is getting banned")]
    public string? Reason { get; init; }
    [NamedCommandParameter("prunedays", "number of days to prune message history")]
    public int PruneDays { get; init; }
}