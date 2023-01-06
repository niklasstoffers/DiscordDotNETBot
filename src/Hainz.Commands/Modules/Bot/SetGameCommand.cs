using Discord.Commands;
using Hainz.Core.Services.Status;

namespace Hainz.Commands.Modules.Bot;

public sealed class SetGameCommand : BotCommandBase
{
    private readonly ActivityService _activityService;

    public SetGameCommand(ActivityService activityService)
    {
        _activityService = activityService;
    }

    [Command("setgame")]
    public async Task SetGameAsync([Remainder] string game) 
    {
        if (await _activityService.SetGameAsync(game))
        {
            await ReplyAsync($"Set game to \"{game}\"");
        }
        else
        {
            await ReplyAsync($"Failed to set game to \"{game}\"");
        }
    }
}