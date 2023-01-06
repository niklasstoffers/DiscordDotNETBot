using Discord.WebSocket;
using MediatR;

namespace Hainz.Events.Notifications.Messages;

public class ReactionAdded : INotification
{
    public SocketReaction Reaction { get; init; }

    public ReactionAdded(SocketReaction reaction)
    {
        Reaction = reaction;
    }
}