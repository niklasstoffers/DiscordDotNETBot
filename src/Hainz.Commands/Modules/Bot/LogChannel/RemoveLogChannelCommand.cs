using Discord.Commands;
using Discord.WebSocket;
using Hainz.Data.Commands.Channel.RemoveLogChannel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Bot.LogChannel;

public class RemoveLogChannelCommand : LogChannelCommandBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RemoveLogChannelCommand> _logger;

    public RemoveLogChannelCommand(IMediator mediator, ILogger<RemoveLogChannelCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Command("remove")]
    public async Task RemoveLogChannelAsync(SocketGuildChannel channel)
    {
        try
        {
            var command = new Data.Commands.Channel.RemoveLogChannel.RemoveLogChannelCommand(channel.Id);
            var result = await _mediator.Send(command);

            await (result switch
            {
                RemoveLogChannelResult.NotALogChannel => Context.Channel.SendMessageAsync("This channel is not a log channel"),
                RemoveLogChannelResult.Success => Context.Channel.SendMessageAsync("Removed channel from log channels"),
                _ => Task.CompletedTask
            });;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to remove log channel");
            await Context.Channel.SendMessageAsync("Internal error while trying to remove log channel");
        }
    }
}