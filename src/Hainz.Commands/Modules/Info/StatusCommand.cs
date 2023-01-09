using System.Text;
using Discord;
using Discord.Commands;
using Hainz.Commands.Embeds;
using Hainz.Commands.Metadata;
using Hainz.Common.Extensions;
using Hainz.Core.Healthchecks;
using Hainz.Core.Healthchecks.Services;
using Hainz.Core.Services.Bot;

namespace Hainz.Commands.Modules.Info;

[CommandName("status")]
[Summary("bots current status including its services")]
public sealed class StatusCommand : InfoCommandBase
{
    private readonly UptimeMonitorService _uptimeMonitorService;
    private readonly DatabaseHealthCheck _dbHealthcheck;
    private readonly RedisHealthCheck _redisHealthcheck;
    private readonly DefaultEmbedTemplateBuilder _defaultTemplateBuilder;

    public StatusCommand(UptimeMonitorService uptimeMonitorService, 
                         DatabaseHealthCheck dbHealthcheck,
                         RedisHealthCheck redisHealthcheck,
                         DefaultEmbedTemplateBuilder defaultTemplateBuilder)
    {
        _uptimeMonitorService = uptimeMonitorService;
        _dbHealthcheck = dbHealthcheck;
        _redisHealthcheck = redisHealthcheck;
        _defaultTemplateBuilder = defaultTemplateBuilder;
    }

    [Command("status")]
    public async Task StatusAsync()
    {
        var uptimeStats = _uptimeMonitorService.UptimeStatistic;
        var contentBuilder = new StringBuilder();
        var embedBuilder = _defaultTemplateBuilder.Build()
            .WithTitle("Hainz Status!");

        contentBuilder.AppendLine("Current service status");
        contentBuilder.AppendLine();

        string gatewayServiceName = Format.Bold("Discord Gateway");
        if (uptimeStats.IsUp && (uptimeStats.LastDowntime == null || uptimeStats.CurrentUptime > TimeSpan.FromHours(1)))
            contentBuilder.AppendLine($"🟢 {gatewayServiceName}: Connected");
        else if (uptimeStats.IsUp)
            contentBuilder.AppendLine($"🟠 {gatewayServiceName}: Down {uptimeStats.CurrentUptime.Minutes} minutes ago");
        else // this case should not happen
            contentBuilder.AppendLine($"🔴 {gatewayServiceName}: Currently down");

        contentBuilder.AppendLine(GetServiceStatusInfo(_dbHealthcheck, "Database Server"));
        contentBuilder.AppendLine(GetServiceStatusInfo(_redisHealthcheck, "Cache Server"));

        contentBuilder.AppendLine("");
        contentBuilder.AppendLine($"{Format.Underline("Uptime Percentage")}: {Math.Round(uptimeStats.UptimePercentage, 2)}%");
        embedBuilder.Description = contentBuilder.ToString();

        embedBuilder
            .AddField("Current uptime", uptimeStats.CurrentUptime.ToTimeString(), true)
            .AddField("First Uptime", uptimeStats.FirstUptime?.ToDateTimeString(), true)
            .AddField("Total downtime duration", uptimeStats.TotalDowntimeDuration.ToTimeString(), true)
            .AddField("Last downtime", uptimeStats.LastDowntime?.ToDateTimeString() ?? "No downtime yet", true)
            .AddField("Last downtime duration", uptimeStats.LastDowntimeDuration?.ToTimeString() ?? "No downtime yet", true)
            .AddField("Total operation time", uptimeStats.TotalOperationTime.ToTimeString(), true);

        await ReplyAsync(embed: embedBuilder.Build());
    }

    private static string GetServiceStatusInfo(RemoteServiceHealthCheckBase healthCheck, string serviceName)
    {
        serviceName = Format.Bold(serviceName);

        if (healthCheck.IsEnabled)
        {
            if (healthCheck.LastChecked == null) 
                return $"⚪ {serviceName}: Not checked yet";
            else
            {
                var timeSinceLastCheck = DateTime.UtcNow - healthCheck.LastChecked.Value;

                if (healthCheck.IsHealthy)
                    return $"🟢 {serviceName}: Operational. Last checked {timeSinceLastCheck.Seconds} seconds ago";
                else
                    return $"🔴 {serviceName}: Down. Last checked {timeSinceLastCheck.Seconds} seconds ago";
            }
        }
        else
        {
            return $"⚫ {serviceName}: No health checks enabled";
        }
    }
}