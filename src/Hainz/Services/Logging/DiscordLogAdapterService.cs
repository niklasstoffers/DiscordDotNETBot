using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Services.Logging;

internal class DiscordLogAdapterService : IHostedService
{
    private DiscordSocketClient _client;
    private ILogger<DiscordLogAdapterService> _adapterLogger;
    private ILogger<DiscordLogSource> _discordLogger;

    public DiscordLogAdapterService(DiscordSocketClient client,
                                    ILogger<DiscordLogAdapterService> adapterLogger,
                                    ILogger<DiscordLogSource> discordLogger)
    {
        _client = client;
        _adapterLogger = adapterLogger;
        _discordLogger = discordLogger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _adapterLogger.LogInformation("Starting DiscordLogAdapterService...");
        _client.Log += DiscordLogEventHandler;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _adapterLogger.LogInformation("Stopping DiscordLogAdapterService...");
        _client.Log -= DiscordLogEventHandler;
        return Task.CompletedTask;
    }

    private Task DiscordLogEventHandler(LogMessage message) 
    {
        var logLevel = message.Severity switch 
        {
            LogSeverity.Verbose => LogLevel.Trace,
            LogSeverity.Debug => LogLevel.Debug,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Critical => LogLevel.Critical,
            _ => LogLevel.None
        };

        _discordLogger.Log(logLevel, 
            message.Exception, 
            message.Message);
        
        return Task.CompletedTask;
    }
}