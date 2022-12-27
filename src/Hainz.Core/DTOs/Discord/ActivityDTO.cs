using Discord;

namespace Hainz.Core.DTOs.Discord;

public sealed class ActivityDTO
{
    public string Name { get; init; } = null!;
    public ActivityType Type { get; set; }
}