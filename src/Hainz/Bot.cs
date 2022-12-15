using Microsoft.Extensions.Hosting;

namespace Hainz;

internal sealed class Bot : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Starting");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Stopping");
        return Task.CompletedTask;
    }
}