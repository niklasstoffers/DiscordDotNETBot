using Hainz.Hosting;

namespace Hainz.Events;

public class EventListener : GatewayServiceBase
{
    private readonly IEnumerable<INotificationSource> _notificationSources;

    public EventListener(IEnumerable<INotificationSource> notificationSources)
    {
        _notificationSources = notificationSources;
    }

    public override async Task StartAsync()
    {
        foreach (var notificationSource in _notificationSources)
            await notificationSource.RegisterAsync();
    }

    public override async Task StopAsync()
    {
        foreach (var notificationSource in _notificationSources)
            await notificationSource.DeregisterAsync();
    }
}