using Microsoft.Extensions.Hosting;

namespace Hainz.Infrastructure.Extensions;

public static class HostEnvironmentExtensions
{
    public static string GetNLogConfigurationFile(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment switch
        {
            var devEnvironment when devEnvironment.IsDevelopment() => "nlog.debug.config",
            _ => "nlog.release.config"
        };
    }
}