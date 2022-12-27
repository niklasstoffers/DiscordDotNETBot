using Discord.Commands;
using Hainz.Services.Discord;

namespace Hainz.Commands.Modules.Bot;

public sealed class SetGameCommand : BotCommandBase
{
    private readonly DiscordActivityService _activityService;

    public SetGameCommand(DiscordActivityService activityService)
    {
        _activityService = activityService;
    }

    [Command("setgame")]
    public async Task SetGameAsync([Remainder]string game) 
    {
        if (await _activityService.SetGameAsync(game))
        {
            await Context.Channel.SendMessageAsync($"Set game to \"{game}\"");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to set game to \"{game}\"");
        }
    }
}