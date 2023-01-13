using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Commands.Preconditions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Admin;

[OnlyInGuild]
[RequireUserPermission(GuildPermission.Administrator)]
[CommandName("setsenddmuponban")]
[Summary("sets whether banned users should receive a ban dm")]
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
    public async Task SetSendDMUponBanAsync([CommandParameter("enable", "true to send a dm upon ban, otherwise false")]bool enable)
    {
        try
        {
            var command = new Data.Commands.Guild.Bans.SetSendDMUponBan.SetSendDMUponBanCommand(Context.Guild.Id, enable);
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