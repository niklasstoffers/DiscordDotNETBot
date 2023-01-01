using Discord;
using Discord.Commands;
using Hainz.Commands.Helpers;
using Hainz.Commands.Metadata;
using Hainz.Common.Helpers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Admin;

[RequireUserPermission(GuildPermission.Administrator)]
public class SetCommandPrefixCommand : AdminCommandBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<SetCommandPrefixCommand> _logger;

    public SetCommandPrefixCommand(IMediator mediator, ILogger<SetCommandPrefixCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Command("setcommandprefix")]
    [Summary("Sets the command prefix for this bot guild-wide")]
    public async Task SetCommandPrefixAsync([CommandParameter(CommandParameterType.Char, "prefix", "The prefix to use")]char prefix)
    {
        var isValidPrefix = CommandPrefixValidator.IsValidPrefix(prefix);
        if (!isValidPrefix)
        {
            var validPrefixes = string.Join(" ", CommandPrefixValidator.ValidPrefixes);
            await Context.Channel.SendMessageAsync($"Invalid command prefix. Command prefix can only be set to [{validPrefixes}]");
            return;
        }

        try
        {
            var command = new Data.Commands.Guild.Commands.SetCommandPrefixCommand(Context.Guild.Id, prefix);
            await _mediator.Send(command);
            await Context.Channel.SendMessageAsync("Command prefix successfully updated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to set command prefix");
            await Context.Channel.SendMessageAsync("Internal error while trying to update command prefix");
        }
    }
}