using Discord;
using Hainz.Events.Notifications.Logs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Logging;

public sealed class DiscordLogAdapterService : INotificationHandler<Log>
{
    private readonly ILogger<DiscordLogAdapterService> _adapterLogger;
    private readonly ILogger<DiscordLogSource> _discordLogger;

    public DiscordLogAdapterService(ILogger<DiscordLogAdapterService> adapterLogger,
                                    ILogger<DiscordLogSource> discordLogger)
    {
        _adapterLogger = adapterLogger;
        _discordLogger = discordLogger;
    }

    public Task Handle(Log notification, CancellationToken cancellationToken)
    {
        _adapterLogger.LogTrace("Received log message from discord");
        var message = notification.Message;

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