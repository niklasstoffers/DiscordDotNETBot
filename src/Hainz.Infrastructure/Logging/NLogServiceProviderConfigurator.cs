using Log = global::NLog;

namespace Hainz.Infrastructure.Logging;

public static class NLogServiceProviderConfigurator 
{
    public static void ReloadConfigWithServiceProvider(IServiceProvider serviceProvider) 
    {
        // NLog catch22 work-around. Reloads all NLog configuration using the specified service provider.
        // This is required because NLog tries to resolve it's dependencies right when being added as a LoggingProvider to the HostBuilder.
        // However at that time the service provider used by the application is still under construction.
        // Thus when using custom targets that require custom dependencies, NLog is not able to resolve them from it's internal service provider.
        // The InitializeTarget() pattern introduced with NLog 5.0 also doesn't work in this case since NLog will automatically resolve unregistered dependencies by itself 
        // if they have a default constructor, completely ignoring the provided ServiceProvider. Furthermore its an inconsitency within the applications general DI flow.

        var nlogServiceProvider = Log.Config.ConfigurationItemFactory.Default.CreateInstance;
        Log.Config.ConfigurationItemFactory.Default.CreateInstance = 
            type => serviceProvider.GetService(type) ?? nlogServiceProvider(type);
        Log.LogManager.Configuration = Log.LogManager.Configuration.Reload();
    }
}