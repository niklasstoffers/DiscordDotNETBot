using MediatR;

namespace Hainz.Core.Events;

public record ApplicationShutdownRequest : INotification;