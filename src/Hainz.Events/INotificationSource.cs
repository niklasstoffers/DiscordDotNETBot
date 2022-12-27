namespace Hainz.Events;

public interface INotificationSource
{
    Task RegisterAsync();
    Task DeregisterAsync();
}