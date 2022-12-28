namespace Hainz.Hosting;

public interface IGatewayServiceHost<out TService> where TService : IGatewayService
{
    public Task StartAsync();
    public Task StopAsync();
}