using Discord.WebSocket;
using MediatR;

namespace Hainz.Events.Notifications.Messages;

public class MessageReceived : INotification
{
    public SocketMessage Message { get; init; }

    public MessageReceived(SocketMessage message)
    {
        Message = message;
    }
}