namespace Hainz.Hosting;

public interface IGatewayService
{
    Task SetupAsync();
    Task StartAsync();
    Task StopAsync();
}