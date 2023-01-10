using Discord;
using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Data.Commands.Guild.Commands.RemoveCommandPrefix;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Admin.CommandPrefix;

[RequireUserPermission(GuildPermission.Administrator)]
[CommandName("remove")]
[Summary("removes a guild specific command prefix")]
public class RemoveCommandPrefixCommand : CommandPrefixCommandBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RemoveCommandPrefixCommand> _logger;

    public RemoveCommandPrefixCommand(IMediator mediator, ILogger<RemoveCommandPrefixCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Command("remove")]
    public async Task RemoveCommandPrefixAsync()
    {
        try
        {
            var command = new Data.Commands.Guild.Commands.RemoveCommandPrefix.RemoveCommandPrefixCommand(Context.Guild.Id);
            var result = await _mediator.Send(command);

            if (result == RemoveCommandPrefixResult.NoCustomPrefix)
                await ReplyAsync("This guild has no custom command prefix");
            else
                await ReplyAsync("Command prefix has been removed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to remove command prefix");
            await ReplyAsync("Internal error while trying to remove command prefix");
        }
    }
}