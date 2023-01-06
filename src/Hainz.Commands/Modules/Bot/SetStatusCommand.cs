using Discord;
using Discord.Commands;
using Hainz.Core.Services.Status;

namespace Hainz.Commands.Modules.Bot;

public sealed class SetStatusCommand : BotCommandBase
{
    private readonly StatusService _statusService;

    public SetStatusCommand(StatusService statusService)
    {
        _statusService = statusService;
    }

    [Command("setstatus")]
    public async Task SetStatusAsync(UserStatus status) 
    {
        if (await _statusService.SetStatusAsync(status)) 
        {
            await ReplyAsync($"Set status to \"{status}\"");
        }
        else
        {
            await ReplyAsync($"Failed to set status to \"{status}\"");
        }
    }
}