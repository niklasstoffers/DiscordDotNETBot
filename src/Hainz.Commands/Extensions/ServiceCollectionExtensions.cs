using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Commands.Extensions;

public static class ServiceCollectionExtensions 
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection) 
    {
        return serviceCollection;
    }
}