using Discord.Commands;
using Discord.WebSocket;
using Hainz.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hainz.Events.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvents(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ClientEventPropagatorDecorator>();
        serviceCollection.AddSingleton<CommandServiceEventPropagatorDecorator>();

        serviceCollection.DecorateService<DiscordSocketClient>((serviceProvider, client) =>
        {
            var eventPropagatorDecorator = serviceProvider.GetRequiredService<ClientEventPropagatorDecorator>();
            eventPropagatorDecorator.Decorate(client);
        });

        serviceCollection.DecorateService<CommandService>((serviceProvider, commandService) =>
        {
            var eventPropagatorDecorator = serviceProvider.GetRequiredService<CommandServiceEventPropagatorDecorator>();
            eventPropagatorDecorator.Decorate(commandService);
        });

        return serviceCollection;
    }

    public static IServiceCollection DecorateService<T>(this IServiceCollection serviceCollection, Action<IServiceProvider, T> decorator)
    {
        var existingDescriptor = serviceCollection.SingleOrDefault(descriptor => descriptor.ServiceType == typeof(T)) ??
            throw new InvalidOperationException($"Cannot decorate service {typeof(T).Name} because it hasn't been registered yet.");
        
        var decoratedDescriptor = new ServiceDescriptor(
            existingDescriptor.ServiceType,
            provider =>
            {
                if (existingDescriptor.GetInstance(provider) is T tServiceInstance)
                {
                    decorator(provider, tServiceInstance);
                    return tServiceInstance;
                }
                else
                    throw new InvalidOperationException($"Couldn't resolve {typeof(T).Name} from existing descriptor");
            },
            existingDescriptor.Lifetime
        );

        serviceCollection.Replace(decoratedDescriptor);
        return serviceCollection;
    }
}