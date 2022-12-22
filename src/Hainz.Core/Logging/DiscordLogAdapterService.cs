using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Logging;

public sealed class DiscordLogAdapterService : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly ILogger<DiscordLogAdapterService> _adapterLogger;
    private readonly ILogger<DiscordLogSource> _discordLogger;

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

#pragma warning disable CA2254
        _discordLogger.Log(logLevel, 
            message.Exception, 
            message.Message);
#pragma warning restore CA2254
        
        return Task.CompletedTask;
    }
}