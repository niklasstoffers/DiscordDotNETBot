using Discord;
using Hainz.Core.DTOs.Discord;

namespace Hainz.Core.Config.Bot;

public sealed class BotConfig 
{
    public string Token { get; init; } = null!;
    public UserStatus? DefaultStatus { get; set; }
    public ActivityDTO? DefaultActivity { get; set; }
}