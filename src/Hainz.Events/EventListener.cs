namespace Hainz.Events;

public class EventListener : IEventListener
{
    private readonly IEnumerable<INotificationSource> _notificationSources;

    public EventListener(IEnumerable<INotificationSource> notificationSources)
    {
        _notificationSources = notificationSources;
    }

    public async Task StartAsync()
    {
        foreach (var notificationSource in _notificationSources)
            await notificationSource.RegisterAsync();
    }

    public async Task StopAsync()
    {
        foreach (var notificationSource in _notificationSources)
            await notificationSource.DeregisterAsync();
    }
}