using Discord;
using Discord.WebSocket;
using Hainz.Events.Notifications.Messages;
using MediatR;

namespace Hainz.Events.NotificationSources.Messages;

public class ReactionAddedNotificationSource : INotificationSource
{
    private readonly DiscordSocketClient _client;
    private readonly IMediator _mediator;

    public ReactionAddedNotificationSource(DiscordSocketClient client,
                                           IMediator mediator)
    {
        _client = client;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _client.ReactionAdded += ReactionAddedAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _client.ReactionAdded -= ReactionAddedAsync;
        return Task.CompletedTask;
    }

    private async Task ReactionAddedAsync(Cacheable<IUserMessage, ulong> message, Cacheable<IMessageChannel, ulong> channel, SocketReaction reaction) =>
        await _mediator.Publish(new ReactionAdded(reaction));
}