using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Logging;
using Hainz.Data.Commands.Channel.RemoveLogChannel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Bot.LogChannel;

[CommandName("remove")]
[Summary("removes a new log channel")]
public class RemoveLogChannelCommand : LogChannelCommandBase
{
    private readonly IMediator _mediator;
    private readonly DiscordChannelLoggerService _loggerService;
    private readonly ILogger<RemoveLogChannelCommand> _logger;

    public RemoveLogChannelCommand(IMediator mediator, 
                                   DiscordChannelLoggerService loggerService,
                                   ILogger<RemoveLogChannelCommand> logger)
    {
        _mediator = mediator;
        _loggerService = loggerService;
        _logger = logger;
    }

    [Command("remove")]
    public async Task RemoveLogChannelAsync([CommandParameter("channel", "the channel to remove")] SocketGuildChannel channel)
    {
        try
        {
            var command = new Data.Commands.Channel.RemoveLogChannel.RemoveLogChannelCommand(channel.Id);
            var result = await _mediator.Send(command);

            if (result == RemoveLogChannelResult.NotALogChannel)
                await ReplyAsync("This channel is not a log channel");
            else if (result == RemoveLogChannelResult.Success)
            {
                await _loggerService.Reload();
                await ReplyAsync("Removed channel from log channels");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to remove log channel");
            await ReplyAsync("Internal error while trying to remove log channel");
        }
    }
}