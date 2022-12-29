using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Hainz.Infrastructure.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddInfrastructure(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureLogging((hostBuilderContext, loggingBuilder) =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);

            var nlogConfigFile = hostBuilderContext.HostingEnvironment.GetNLogConfigurationFile();
            loggingBuilder.AddNLog(nlogConfigFile);
        });

        return hostBuilder;
    }
}