using Discord;
using MediatR;

namespace Hainz.Events.Notifications.Commands;

public class Log : INotification
{
    public LogMessage Message { get; init; }

    public Log(LogMessage message)
    {
        Message = message;
    }
}