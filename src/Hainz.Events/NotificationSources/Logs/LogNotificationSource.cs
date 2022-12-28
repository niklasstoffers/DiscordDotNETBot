using Discord;
using Discord.WebSocket;
using Hainz.Events.Notifications.Logs;
using MediatR;

namespace Hainz.Events.NotificationSources.Logs;

public class LogNotificationSource : INotificationSource
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;

    public LogNotificationSource(DiscordSocketClient client,
                                 IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _client.Log += LogAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _client.Log -= LogAsync;
        return Task.CompletedTask;
    }

    private async Task LogAsync(LogMessage message) => 
        await _mediator.Publish(new Log(message));
}
