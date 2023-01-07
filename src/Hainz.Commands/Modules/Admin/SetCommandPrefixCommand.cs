using Discord;
using Discord.Commands;
using Hainz.Commands.Helpers;
using Hainz.Commands.Metadata;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Admin;

[RequireUserPermission(GuildPermission.Administrator)]
[CommandName("setcommandprefix")]
[Summary("sets a guild specific command prefix")]
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
    public async Task SetCommandPrefixAsync([CommandParameter("prefix", "prefix to use")] char prefix)
    {
        var isValidPrefix = CommandPrefixValidator.IsValidPrefix(prefix);
        if (!isValidPrefix)
        {
            var validPrefixes = string.Join(" ", CommandPrefixValidator.ValidPrefixes);
            await ReplyAsync($"Invalid command prefix. Command prefix can only be set to [{validPrefixes}]");
            return;
        }

        try
        {
            var command = new Data.Commands.Guild.Commands.SetCommandPrefixCommand(Context.Guild.Id, prefix);
            await _mediator.Send(command);
            await ReplyAsync("Command prefix successfully updated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to set command prefix");
            await ReplyAsync("Internal error while trying to update command prefix");
        }
    }
}