using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
using Hainz.Data.Commands.Channel.AddLogChannel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Bot.LogChannel;

[CommandName("add")]
[Summary("adds a new log channel")]
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
    public async Task AddLogChannelAsync([CommandParameter("channel", "the channel to add")] SocketGuildChannel channel)
    {
        try
        {
            var command = new Data.Commands.Channel.AddLogChannel.AddLogChannelCommand(channel.Id);
            var result = await _mediator.Send(command);

            await (result switch 
            {
                AddLogChannelResult.AlreadyALogChannel => ReplyAsync("This channel is already a log channel"),
                AddLogChannelResult.Success => ReplyAsync("Added channel to log channels"),
                _ => Task.CompletedTask
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to add log channel");
            await ReplyAsync("Internal error while trying to add log channel");
        }
    }
}