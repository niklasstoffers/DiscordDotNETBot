using Discord;
using Hainz.DTOs.Discord;

namespace Hainz.Config.Bot;

public sealed class BotConfig 
{
    public string Token { get; init; } = null!;
    public UserStatus? DefaultStatus { get; set; }
    public ActivityDTO? DefaultActivity { get; set; }
}