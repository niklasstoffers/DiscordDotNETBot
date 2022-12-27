using Discord.WebSocket;
using Hainz.Events.Notifications.Connection;
using MediatR;

namespace Hainz.Events.NotificationSources.Connection;

public class DisconnectedNotificationSource : INotificationSource
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;

    public DisconnectedNotificationSource(DiscordSocketClient client,
                                          IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _client.Disconnected += DisconnectedAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _client.Disconnected -= DisconnectedAsync;
        return Task.CompletedTask;
    }

    private async Task DisconnectedAsync(Exception ex) => 
        await _mediator.Publish(new Disconnected(ex));
}
