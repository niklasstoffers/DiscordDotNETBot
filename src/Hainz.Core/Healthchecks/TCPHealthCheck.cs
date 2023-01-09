using System.Net.Sockets;

namespace Hainz.Core.Healthchecks;

public sealed class TCPHealthCheck : IDisposable
{
    private readonly string _hostname;
    private readonly int _port;
    private readonly TCPHealthCheckOptions _options;
    private readonly object _lock = new();
    private CancellationTokenSource? _stopCTS;
    private Task? _healthCheckTask;

    public bool IsHealthy { get; private set; } = true;
    public DateTime? LastChecked { get; private set; }

    public TCPHealthCheck(string hostname, int port, TCPHealthCheckOptions configuration)
    {
        _hostname = hostname;
        _port = port;
        _options = configuration;
    }

    public Task StartAsync()
    {
        if (_stopCTS?.IsCancellationRequested ?? true)
        {
            _stopCTS = new CancellationTokenSource();
            _healthCheckTask = Task.Run(async () => await HealthCheck(_stopCTS.Token));
        }

        return Task.CompletedTask;
    }

    public async Task StopAsync()
    {
        _stopCTS?.Cancel();

        try
        {
            await (_healthCheckTask ?? Task.CompletedTask);
        }
        catch { }
    }

    public void Dispose()
    {
        _stopCTS?.Dispose();
    }

    private async Task HealthCheck(CancellationToken cancelToken)
    {
        await Task.Delay(_options.InitialTimeout * 1000, cancelToken);

        TcpClient? currentClient = null;
        Task? connectTask = null;

        try 
        {
            while(!cancelToken.IsCancellationRequested)
            {
                var timeoutCompletionSource = new TaskCompletionSource();
                using var connectCTS = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
                using var timeoutRegistration = connectCTS.Token.Register(() => timeoutCompletionSource.SetResult());

                if (connectTask == null)
                {
                    currentClient = new TcpClient();
                    connectTask = currentClient.ConnectAsync(_hostname, _port);
                }

                connectCTS.CancelAfter(_options.Timeout * 1000);

                bool isHealthy = false;
                try
                {
                    isHealthy = connectTask == await Task.WhenAny(timeoutCompletionSource.Task, connectTask) && currentClient!.Connected;
                }
                catch
                {
                    currentClient?.Dispose();
                    connectTask = null;
                }

                lock(_lock)
                {
                    IsHealthy = isHealthy;
                    LastChecked = DateTime.UtcNow;
                }

                if (connectTask?.IsCompleted ?? false)
                {
                    currentClient?.Dispose();
                    connectTask = null;
                }

                if (isHealthy)
                    await Task.Delay(_options.Interval * 1000, cancelToken);
                else
                    await Task.Delay(_options.UnhealthyInterval * 1000, cancelToken);
            }
        }
        finally
        {
            currentClient?.Dispose();
        }
    }
}