using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Logging;
using Hainz.Data.Commands.Channel.AddLogChannel;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Bot.LogChannel;

[CommandName("add")]
[Summary("adds a new log channel")]
public class AddLogChannelCommand : LogChannelCommandBase
{
    private readonly IMediator _mediator;
    private readonly DiscordChannelLoggerService _loggerService;
    private readonly ILogger<AddLogChannelCommand> _logger;

    public AddLogChannelCommand(IMediator mediator, 
                                DiscordChannelLoggerService loggerService, 
                                ILogger<AddLogChannelCommand> logger)
    {
        _mediator = mediator;
        _loggerService = loggerService;
        _logger = logger;
    }

    [Command("add")]
    public async Task AddLogChannelAsync([CommandParameter("channel", "the channel to add")] SocketGuildChannel channel)
    {
        try
        {
            var command = new Data.Commands.Channel.AddLogChannel.AddLogChannelCommand(channel.Id);
            var result = await _mediator.Send(command);

            if (result == AddLogChannelResult.AlreadyALogChannel)
                await ReplyAsync("This channel is already a log channel");
            else if(result == AddLogChannelResult.Success)
            {
                await _loggerService.Reload();
                await ReplyAsync("Added channel to log channels");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to add log channel");
            await ReplyAsync("Internal error while trying to add log channel");
        }
    }
}