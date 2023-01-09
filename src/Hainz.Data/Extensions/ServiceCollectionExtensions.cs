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
using EFCoreSecondLevelCacheInterceptor;
using Hainz.Data.Configuration.Caching;
using EasyCaching.Core.Configurations;
using MessagePack.Resolvers;
using MessagePack.Formatters;
using MessagePack;

namespace Hainz.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection serviceCollection, PersistenceConfiguration persistenceConfigurartion, CachingConfiguration cachingConfiguration)
    {
        new PersistenceConfigurationValidator().ValidateAndThrow(persistenceConfigurartion);
        
        var connectionString = persistenceConfigurartion.ToConnectionString();
        serviceCollection.AddDbContext<HainzDbContext>((serviceProvider, opt) =>
            opt.UseNpgsql(connectionString)
               .AddInterceptors(serviceProvider.GetRequiredService<SecondLevelCacheInterceptor>()));
        serviceCollection.AddCaching(cachingConfiguration);

        serviceCollection.AddSingleton(persistenceConfigurartion);
        serviceCollection.AddSingleton(cachingConfiguration);

        serviceCollection.AddTransient<DbMigrationHelper>();
        serviceCollection.AddTransient<DbInitializer>();
        serviceCollection.AddTransient<GuildService>();
        serviceCollection.AddTransient<ChannelService>();

        var currentAssembly = Assembly.GetExecutingAssembly();
        serviceCollection.AddMediatR(currentAssembly);

        return serviceCollection;
    }

    private static IServiceCollection AddCaching(this IServiceCollection serviceCollection, CachingConfiguration cachingConfiguration)
    {
        new CachingConfigurationValidator().ValidateAndThrow(cachingConfiguration);

        serviceCollection.AddEFSecondLevelCache(options =>
            options.UseEasyCachingCoreProvider(cachingConfiguration.ProviderName, isHybridCache: false)
                .UseCacheKeyPrefix(cachingConfiguration.CacheKeyPrefix)
                .CacheAllQueries(cachingConfiguration.ExpirationMode, TimeSpan.FromSeconds(cachingConfiguration.TimeoutSeconds))
                .DisableLogging(true)
        );

        const string messagePackSerializerName = "Pack";
        serviceCollection.AddEasyCaching(options =>
            options.UseRedis(redisOptions =>
            {
                redisOptions.DBConfig.AllowAdmin = true;
                redisOptions.DBConfig.SyncTimeout = cachingConfiguration.Redis.SyncTimeout;
                redisOptions.DBConfig.AsyncTimeout = cachingConfiguration.Redis.AsyncTimeout;
                redisOptions.DBConfig.ConnectionTimeout = cachingConfiguration.Redis.ConnectionTimeout;
                redisOptions.DBConfig.Endpoints.Add(new ServerEndPoint(cachingConfiguration.Redis.Hostname, cachingConfiguration.Redis.Port));
                redisOptions.EnableLogging = false;
                redisOptions.SerializerName = messagePackSerializerName;
            }, cachingConfiguration.ProviderName)
            .WithMessagePack(serializerOptions =>
            {
                serializerOptions.EnableCustomResolver = true;
                serializerOptions.CustomResolvers = CompositeResolver.Create(
                    new IMessagePackFormatter[]
                    {
                        DBNullFormatter.Instance
                    },
                    new IFormatterResolver[]
                    {
                        NativeDateTimeResolver.Instance,
                        ContractlessStandardResolver.Instance,
                        StandardResolverAllowPrivate.Instance
                    }
                );
            },
            messagePackSerializerName)
        );

        return serviceCollection;
    }
}