using Discord;
using MediatR;

namespace Hainz.Events.Notifications.Logs;

public record Log(LogMessage Message) : INotification;