using Discord;

namespace Hainz.DTOs.Discord;

public sealed class ActivityDTO
{
    public string Name { get; init; } = null!;
    public ActivityType Type { get; set; }
}