using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Status;

namespace Hainz.Commands.Modules.Bot;

[CommandName("setgame")]
[Summary("sets the bots current game")]
public sealed class SetGameCommand : BotCommandBase
{
    private readonly ActivityService _activityService;

    public SetGameCommand(ActivityService activityService)
    {
        _activityService = activityService;
    }

    [Command("setgame")]
    public async Task SetGameAsync([Remainder, CommandParameter("game", "the game name")] string game) 
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