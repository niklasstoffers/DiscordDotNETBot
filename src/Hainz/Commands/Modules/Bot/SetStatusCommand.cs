using Discord;
using Discord.Commands;
using Hainz.Services.Discord;

namespace Hainz.Commands.Modules.Bot;

public sealed class SetStatusCommand : BotCommandBase
{
    private readonly DiscordStatusService _statusService;

    public SetStatusCommand(DiscordStatusService statusService)
    {
        _statusService = statusService;
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