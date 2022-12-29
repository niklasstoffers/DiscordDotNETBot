using FluentValidation;
using Hainz.Persistence.Configuration;
using Hainz.Persistence.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hainz.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, PersistenceConfiguration config)
    {
        new PersistenceConfigurationValidator().ValidateAndThrow(config);
        
        var connectionString = config.ToConnectionString();
        serviceCollection.AddEntityFrameworkNpgsql().AddDbContext<HainzDbContext>(opt =>
            opt.UseNpgsql(connectionString));

        return serviceCollection;
    }
}