using Discord.Commands;
using Hainz.Commands.Metadata;

namespace Hainz.Commands.Modules.Admin.Parameters;

[NamedArgumentType]
public sealed class BanOptionsParameter
{
    [NamedCommandParameter(CommandParameterType.Text, "reason", "Reason why the user is getting banned")]
    public string? Reason { get; init; }
    [NamedCommandParameter(CommandParameterType.Number, "message prune days", "Prunes messages from the last x days. Must be in range: [0-7]")]
    public int PruneDays { get; init; }
}