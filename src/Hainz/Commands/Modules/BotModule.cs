using Discord;
using Discord.Commands;
using Hainz.Commands.Preconditions;
using Hainz.Services.Discord;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules;

[RequireBotAdminPermission]
public class BotModule : ModuleBase<SocketCommandContext>
{
    private readonly DiscordStatusService _statusService;
    private readonly DiscordActivityService _activityService;

    public BotModule(DiscordStatusService statusService,
                     DiscordActivityService activityService) 
    {
        _statusService = statusService;
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

    [Command("setstatus")]
    public async Task SetStatusAsync(UserStatus status) 
    {
        if (await _statusService.SetStatusAsync(status)) 
        {
            await Context.Channel.SendMessageAsync($"Set status to \"{status}\"");
        }
        else
        {
            await Context.Channel.SendMessageAsync($"Failed to set status to \"{status}\"");
        }
    }
}