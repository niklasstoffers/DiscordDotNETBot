namespace Hainz.Events;

public interface IEventListener
{
    Task StartAsync();
    Task StopAsync();
}