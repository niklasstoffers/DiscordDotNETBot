using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Hosting.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayService<TService>(this IServiceCollection serviceCollection) where TService : class, IGatewayService
    {
        serviceCollection.AddSingleton<TService>();
        serviceCollection.AddSingleton<IGatewayServiceHost<IGatewayService>, GatewayServiceHost<TService>>();

        return serviceCollection;
    }
}