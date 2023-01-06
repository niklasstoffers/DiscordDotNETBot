using Discord;
using Discord.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Admin;

[RequireUserPermission(GuildPermission.Administrator)]
public sealed class SetSendDMUponBanCommand : AdminCommandBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SetSendDMUponBanCommand> _logger;

    public SetSendDMUponBanCommand(IMediator mediator, ILogger<SetSendDMUponBanCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Command("setsenddmuponban")]
    public async Task SetSendDMUponBanAsync(bool enable)
    {
        try
        {
            var command = new Data.Commands.Guild.Bans.SetSendDMUponBanCommand(Context.Guild.Id, enable);
            await _mediator.Send(command);

            if (enable)
                await ReplyAsync("Sending DM upon ban enabled");
            else
                await ReplyAsync("Sending DM upon ban disabled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to update SendDMUponBan setting");
            await ReplyAsync("Internal error while trying to update setting");
        }
    }
}