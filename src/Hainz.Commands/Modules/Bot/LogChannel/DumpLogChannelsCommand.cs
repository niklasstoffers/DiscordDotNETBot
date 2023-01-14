using Discord.Commands;
using Hainz.Commands.Metadata;
using Hainz.Core.Services.Logging;
using Hainz.Data.Commands.Channel.RemoveLogChannels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands.Modules.Bot.LogChannel;

[CommandName("dump")]
[Summary("dumps all log channels")]
public class DumpLogChannelsCommand : LogChannelCommandBase
{
    private readonly IMediator _mediator;
    private readonly DiscordChannelLoggerService _loggerService;
    private readonly ILogger<DumpLogChannelsCommand> _logger;

    public DumpLogChannelsCommand(IMediator mediator, 
                                  DiscordChannelLoggerService loggerService,
                                  ILogger<DumpLogChannelsCommand> logger)
    {
        _mediator = mediator;
        _loggerService = loggerService;
        _logger = logger;
    }

    [Command("dump")]
    public async Task DumpLogChannelsAsync()
    {
        try
        {
            var command = new RemoveLogChannelsCommand();
            var channelsRemoved = await _mediator.Send(command);

            if (channelsRemoved > 0)
                await _loggerService.Reload();

            await ReplyAsync($"Number of channels removed: {channelsRemoved}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while trying to dump log channels");
            await ReplyAsync("Internal error while trying to dump log channels");
        }
    }
}