using Hainz.Core.Services.Logging;
using NLog;
using NLog.Targets;

namespace Hainz.Infrastructure.Logging.Targets;

[Target("DiscordChannel")]
public sealed class DiscordChannelLogTarget : TargetWithLayout
{
    private readonly DiscordChannelLoggerService? _channelLoggerService;

    // Nlog catch22 workaround
    public DiscordChannelLogTarget() { }

    public DiscordChannelLogTarget(DiscordChannelLoggerService channelLoggerService)
    {
        _channelLoggerService = channelLoggerService;
    }

    protected override void Write(LogEventInfo logEvent)
    {
        var message = Layout.Render(logEvent);
        _channelLoggerService?.LogMessage(message);
    }
}