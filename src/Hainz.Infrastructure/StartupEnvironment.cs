using Hainz.Infrastructure.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NLog;
using NLog.Extensions.Logging;
using MLog = Microsoft.Extensions.Logging;

namespace Hainz.Infrastructure;

public sealed class StartupEnvironment 
{
    public IHostEnvironment HostEnvironment { get; }
    public MLog.ILogger Logger { get; }

    public StartupEnvironment(string environmentName)
    {
        HostEnvironment = new HostingEnvironment() 
        {
            EnvironmentName = environmentName
        };

        var nlogConfigFile = HostEnvironment.GetNLogConfigurationFile();
        var nlogLogFactory = LogManager.LoadConfiguration(nlogConfigFile);
        var nlogProviderOptions = new NLogProviderOptions();
        var nlogProvider = new NLogLoggerProvider(nlogProviderOptions, nlogLogFactory);
        var loggerFactory = new NLogLoggerFactory(nlogProvider);
        var loggerName = typeof(StartupEnvironment).FullName;

        Logger = loggerFactory.CreateLogger(loggerName);
    }
}