using Discord.WebSocket;
using Hainz.Events.Notifications.Connection;
using MediatR;

namespace Hainz.Events.NotificationSources.Connection;

public class ReadyNotificationSource : INotificationSource
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;

    public ReadyNotificationSource(DiscordSocketClient client,
                                   IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _client.Ready += ReadyAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _client.Ready -= ReadyAsync;
        return Task.CompletedTask;
    }

    private async Task ReadyAsync() =>
        await _mediator.Publish(new Ready());
}
