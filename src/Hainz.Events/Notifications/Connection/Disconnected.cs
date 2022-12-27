using MediatR;

namespace Hainz.Events.Notifications.Connection;

public class Disconnected : INotification
{
    public Exception DisconnectException { get; init; }

    public Disconnected(Exception exception)
    {
        DisconnectException = exception;
    }
}