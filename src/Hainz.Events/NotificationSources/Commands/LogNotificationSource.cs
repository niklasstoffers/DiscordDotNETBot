using Discord;
using Discord.Commands;
using Hainz.Events.Notifications.Commands;
using MediatR;

namespace Hainz.Events.NotificationSources.Commands;

public class LogNotificationSource : INotificationSource
{
    private readonly CommandService _commandService;
    private readonly IMediator _mediator;

    public LogNotificationSource(CommandService commandService,
                                 IMediator mediator)
    {
        _commandService = commandService;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _commandService.Log += LogAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _commandService.Log -= LogAsync;
        return Task.CompletedTask;
    }

    private async Task LogAsync(LogMessage message) => 
        await _mediator.Publish(new Log(message));
}
