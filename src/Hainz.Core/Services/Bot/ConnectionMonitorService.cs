using Hainz.Core.DTOs;
using Hainz.Events.Notifications.Connection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Bot;

public sealed class ConnectionMonitorService : INotificationHandler<Connected>, INotificationHandler<Disconnected>
{
    private readonly UptimeMonitorService _uptimeMonitor;
    private readonly ILogger<ConnectionMonitorService> _logger;

    public ConnectionMonitorService(UptimeMonitorService uptimeMonitor, ILogger<ConnectionMonitorService> logger)
    {
        _uptimeMonitor = uptimeMonitor;
        _logger = logger;
    }

    public Task Handle(Connected notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connected to gateway");
        _uptimeMonitor.Connect();

        return Task.CompletedTask;
    }

    public Task Handle(Disconnected notification, CancellationToken cancellationToken)
    {
        _logger.LogCritical(notification.DisconnectException, "Disconnected from gateway");
        _uptimeMonitor.Disconnect();

        return Task.CompletedTask;
    }
}