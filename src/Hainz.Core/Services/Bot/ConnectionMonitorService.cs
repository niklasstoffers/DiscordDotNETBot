using System.Net;
using Discord.Net;
using Hainz.Core.DTOs;
using Hainz.Core.Events;
using Hainz.Events.Notifications.Connection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Bot;

public sealed class ConnectionMonitorService : INotificationHandler<Connected>, INotificationHandler<Disconnected>
{
    private readonly UptimeMonitorService _uptimeMonitor;
    private readonly IMediator _mediator;
    private readonly ILogger<ConnectionMonitorService> _logger;

    public ConnectionMonitorService(UptimeMonitorService uptimeMonitor, 
                                    IMediator mediator,
                                    ILogger<ConnectionMonitorService> logger)
    {
        _uptimeMonitor = uptimeMonitor;
        _mediator = mediator;
        _logger = logger;
    }

    public Task Handle(Connected notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connected to gateway");
        _uptimeMonitor.Connect();

        return Task.CompletedTask;
    }

    public async Task Handle(Disconnected notification, CancellationToken cancellationToken)
    {
        _logger.LogCritical(notification.DisconnectException, "Disconnected from gateway");
        _uptimeMonitor.Disconnect();

        if (notification.DisconnectException is HttpException httpException && 
            httpException.HttpCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogCritical("Bot token was not accepted by Discord. Got HTTP response with code Unauthorized");
            await _mediator.Publish(new ApplicationShutdownRequest(), cancellationToken);
        }
    }
}