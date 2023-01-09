using Hainz.Core.Config;
using Hainz.Hosting;

namespace Hainz.Core.Healthchecks;

public abstract class RemoteServiceHealthCheckBase : GatewayServiceBase, IDisposable
{
    protected HealthCheckConfiguration? Configuration { get; init; }
    protected TCPHealthCheck TCPHealthCheck { get; init; }

    public bool IsEnabled => Configuration?.IsEnabled ?? false;
    public bool IsHealthy => TCPHealthCheck.IsHealthy;
    public DateTime? LastChecked => TCPHealthCheck.LastChecked;

    public RemoteServiceHealthCheckBase(string hostname, int port, HealthCheckConfiguration? configuration)
    {
        var healthCheckOptions = new TCPHealthCheckOptions()
        {
            InitialTimeout = configuration?.InitialTimeout ?? default,
            Interval = configuration?.Interval ?? default,
            Timeout = configuration?.Timeout ?? default,
            UnhealthyInterval = configuration?.UnhealthyInterval ?? default
        };

        TCPHealthCheck = new TCPHealthCheck(hostname, port, healthCheckOptions);
        Configuration = configuration;
    }

    public void Dispose()
    {
        TCPHealthCheck.Dispose();
        GC.SuppressFinalize(this);
    }
}