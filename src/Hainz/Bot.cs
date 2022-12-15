using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz;

internal sealed class Bot : IHostedService
{
    private ILogger<Bot> _logger;

    public Bot(ILogger<Bot> logger) 
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping");
        return Task.CompletedTask;
    }
}