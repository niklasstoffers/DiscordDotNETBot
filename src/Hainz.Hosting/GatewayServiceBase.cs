namespace Hainz.Hosting;

public abstract class GatewayServiceBase : IGatewayService
{
    public virtual Task SetupAsync() => Task.CompletedTask;
    public virtual Task StartAsync() => Task.CompletedTask;
    public virtual Task StopAsync() => Task.CompletedTask;
}
