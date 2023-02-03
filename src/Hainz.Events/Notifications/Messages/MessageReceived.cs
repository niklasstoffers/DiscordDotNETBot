using Discord.WebSocket;
using MediatR;

namespace Hainz.Events.Notifications.Messages;

public record MessageReceived(SocketMessage Message) : INotification;