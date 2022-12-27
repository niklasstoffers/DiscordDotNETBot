using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}