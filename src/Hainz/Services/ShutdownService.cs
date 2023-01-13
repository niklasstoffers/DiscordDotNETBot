using Hainz.Core.Events;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Services;

public class ShutdownService : INotificationHandler<ApplicationShutdownRequest>
{
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<ShutdownService> _logger;

    public ShutdownService(IHostApplicationLifetime appLifetime, ILogger<ShutdownService> logger)
    {
        _appLifetime = appLifetime;
        _logger = logger;
    }

    public Task Handle(ApplicationShutdownRequest notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received application shutdown request");
        _appLifetime.StopApplication();
        return Task.CompletedTask;
    }
}