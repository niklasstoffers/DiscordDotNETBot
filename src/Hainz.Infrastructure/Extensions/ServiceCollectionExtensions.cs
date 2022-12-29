using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceProvider)
    {
        return serviceProvider;
    }
}