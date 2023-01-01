using Discord.Commands;
using Discord.WebSocket;
using Hainz.Data.Commands.Channel.AddLogChannel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Bot.LogChannel;

public class AddLogChannelCommand : LogChannelCommandBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AddLogChannelCommand> _logger;

    public AddLogChannelCommand(IMediator mediator, ILogger<AddLogChannelCommand> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Command("add")]
    public async Task AddLogChannelAsync(SocketGuildChannel channel)
    {
        try
        {
            var command = new Data.Commands.Channel.AddLogChannel.AddLogChannelCommand(channel.Id);
            var result = await _mediator.Send(command);

            await (result switch 
            {
                AddLogChannelResult.AlreadyALogChannel => Context.Channel.SendMessageAsync("This channel is already a log channel"),
                AddLogChannelResult.Success => Context.Channel.SendMessageAsync("Added channel to log channels"),
                _ => Task.CompletedTask
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to add log channel");
            await Context.Channel.SendMessageAsync("Internal error while trying to add log channel");
        }
    }
}