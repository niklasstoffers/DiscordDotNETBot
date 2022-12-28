using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Discord;

namespace Hainz.Commands.Modules.Bot;

public sealed class SetStatusCommand : BotCommandBase
{
    private readonly DiscordStatusService _statusService;

    public SetStatusCommand(DiscordStatusService statusService)
    {
        _statusService = statusService;
    }

    [Command("setstatus")]
    [Summary("Sets the bots status")]
    public async Task SetStatusAsync([CommandParameter(CommandParameterType.UserStatus, "status", "The status")]UserStatus status) 
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