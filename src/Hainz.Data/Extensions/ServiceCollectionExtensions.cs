using FluentValidation;
using Hainz.Data.Configuration;
using Hainz.Data.Helpers;
using Hainz.Data.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using Hainz.Data.Services.Guild;
using Hainz.Data.Services.Channel;

namespace Hainz.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, PersistenceConfiguration config)
    {
        new PersistenceConfigurationValidator().ValidateAndThrow(config);
        
        var connectionString = config.ToConnectionString();
        serviceCollection.AddDbContext<HainzDbContext>(opt =>
            opt.UseNpgsql(connectionString));

        serviceCollection.AddTransient<DbMigrationHelper>();
        serviceCollection.AddTransient<DbInitializer>();
        serviceCollection.AddTransient<GuildService>();
        serviceCollection.AddTransient<ChannelService>();

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddMediatR(currentAssembly);

        return serviceCollection;
    }
}