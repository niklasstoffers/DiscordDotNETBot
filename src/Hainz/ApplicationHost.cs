using Hainz.Events;
using Hainz.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz;

public sealed class ApplicationHost : IHostedService
{
    private readonly Bot _bot;
    private readonly IEventListener _eventListener;
    private readonly IEnumerable<IGatewayServiceHost<IGatewayService>> _gatewayServiceHosts;
    private readonly ILogger<ApplicationHost> _logger;

    public ApplicationHost(Bot bot,
                           IEventListener eventListener,
                           IEnumerable<IGatewayServiceHost<IGatewayService>> gatewayServiceHosts,
                           ILogger<ApplicationHost> logger)
    {
        _bot = bot;
        _eventListener = eventListener;
        _gatewayServiceHosts = gatewayServiceHosts;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting application host");

        await _bot.StartAsync(cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        await _eventListener.StartAsync();

        foreach (var serviceHost in _gatewayServiceHosts) 
        {
            await serviceHost.StartAsync();
        }

        _logger.LogInformation("Application host started");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping application host");

        foreach (var serviceHost in _gatewayServiceHosts)
        {
            await serviceHost.StopAsync();
        }

        await _bot.StopAsync(cancellationToken);
        _logger.LogInformation("Application host stopped");
    }
}