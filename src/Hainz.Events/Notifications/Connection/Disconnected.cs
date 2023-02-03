using MediatR;

namespace Hainz.Events.Notifications.Connection;

public record Disconnected(Exception DisconnectException) : INotification;