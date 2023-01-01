using Discord;
using Discord.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using Hainz.Commands.Metadata;

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
    [Summary("Guild wide setting whether banned users will get a DM by the Bot upon ban")]
    public async Task SetSendDMUponBanAsync([CommandParameter(CommandParameterType.Bool, "enable", "Whether to enable or disable setting")]bool enable)
    {
        try
        {
            var command = new Data.Commands.Guild.Bans.SetSendDMUponBanCommand(Context.Guild.Id, enable);
            await _mediator.Send(command);

            if (enable)
                await Context.Channel.SendMessageAsync("Sending DM upon ban enabled");
            else
                await Context.Channel.SendMessageAsync("Sending DM upon ban disabled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to update SendDMUponBan setting");
            await Context.Channel.SendMessageAsync("Internal error while trying to update setting");
        }
    }
}