using Discord.Commands;

namespace Hainz.Commands.Modules.Admin.Parameters;

[NamedArgumentType]
public sealed class BanOptionsParameter
{
    public string? Reason { get; init; }
    public int PruneDays { get; init; }
}