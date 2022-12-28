using Discord.WebSocket;
using Hainz.Events.Notifications.Connection;
using MediatR;

namespace Hainz.Events.NotificationSources.Connection;

public class ConnectedNotificationSource : INotificationSource
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;

    public ConnectedNotificationSource(DiscordSocketClient client,
                                       IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _client.Connected += ConnectedAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _client.Connected -= ConnectedAsync;
        return Task.CompletedTask;
    }

    private async Task ConnectedAsync() => 
        await _mediator.Publish(new Connected());
}
