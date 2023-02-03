using Discord;
using MediatR;

namespace Hainz.Events.Notifications.Commands;

public record Log(LogMessage Message) : INotification;