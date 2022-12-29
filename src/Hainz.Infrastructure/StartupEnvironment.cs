using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NLog;

namespace Hainz.Infrastructure;

public sealed class StartupEnvironment 
{
    public IHostEnvironment HostEnvironment { get; }
    public Logger Logger { get; }

    public StartupEnvironment(string environmentName)
    {
        HostEnvironment = new HostingEnvironment() 
        {
            EnvironmentName = environmentName
        };

        string nlogConfigFile = "nlog.debug.config";
        if (HostEnvironment.IsProduction())
            nlogConfigFile = "nlog.release.config";
        
        var loggerFactory = LogManager.LoadConfiguration(nlogConfigFile);
        Logger = loggerFactory.GetCurrentClassLogger();
    }
}