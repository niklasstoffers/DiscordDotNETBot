using Discord.WebSocket;
using Hainz.Events.Notifications.Messages;
using MediatR;

namespace Hainz.Events.NotificationSources.Messages;

public class MessageReceivedNotificationSource : INotificationSource
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;

    public MessageReceivedNotificationSource(DiscordSocketClient client,
                                             IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _client.MessageReceived += MessageReceivedAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _client.MessageReceived -= MessageReceivedAsync;
        return Task.CompletedTask;
    }

    private async Task MessageReceivedAsync(SocketMessage message) => 
        await _mediator.Publish(new MessageReceived(message));
}
