using Hainz.Events.NotificationSources.Commands;
using Hainz.Events.NotificationSources.Connection;
using Hainz.Events.NotificationSources.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Events.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvents(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<INotificationSource, CommandExecutedNotificationSource>();
        serviceCollection.AddSingleton<INotificationSource, LogNotificationSource>();
        serviceCollection.AddSingleton<INotificationSource, ConnectedNotificationSource>();
        serviceCollection.AddSingleton<INotificationSource, DisconnectedNotificationSource>();
        serviceCollection.AddSingleton<INotificationSource, ReadyNotificationSource>();
        serviceCollection.AddSingleton<INotificationSource, NotificationSources.Logs.LogNotificationSource>();
        serviceCollection.AddSingleton<INotificationSource, MessageReceivedNotificationSource>();

        serviceCollection.AddSingleton<IEventListener, EventListener>();

        return serviceCollection;
    }
}