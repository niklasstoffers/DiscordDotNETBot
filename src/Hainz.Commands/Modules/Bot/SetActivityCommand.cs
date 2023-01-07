using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Status;

namespace Hainz.Commands.Modules.Bot;

[CommandName("setactivity")]
[Summary("sets the bots current activity")]
public sealed class SetActivityCommand : BotCommandBase
{
    private readonly ActivityService _activityService;

    public SetActivityCommand(ActivityService activityService)
    {
        _activityService = activityService;
    }

    [Command("setactivity")]
    public async Task SetActivityAsync([CommandParameter("type", "type of the activity")] ActivityType type, 
                                       [CommandParameter("name", "activity name")] string name)
    {
        if (await _activityService.SetGameAsync(name, type))
        {
            await ReplyAsync("Activity successfully updated");
        }
        else
        {
            await ReplyAsync("Failed to update activity");
        }
    }
}