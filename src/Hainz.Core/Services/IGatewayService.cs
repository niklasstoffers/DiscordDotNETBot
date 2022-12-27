namespace Hainz.Core.Services;

public interface IGatewayService
{
    Task StartAsync(bool isRestart);
    Task StopAsync();
}