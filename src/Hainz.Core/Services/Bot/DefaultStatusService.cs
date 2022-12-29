using Hainz.Core.Config.Bot;
using Hainz.Core.Services.Status;
using Hainz.Events.Notifications.Connection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Bot;

public sealed class DefaultStatusService : INotificationHandler<Ready>
{
    private readonly StatusService _statusService;
    private readonly ActivityService _activityService;
    private readonly BotConfig _botConfig;
    private readonly ILogger<DefaultStatusService> _logger;

    public DefaultStatusService(StatusService statusService,
                                ActivityService activityService,
                                BotConfig botConfig,
                                ILogger<DefaultStatusService> logger)
    {
        _statusService = statusService;
        _activityService = activityService;
        _botConfig = botConfig;
        _logger = logger;
    }

    public async Task Handle(Ready notification, CancellationToken cancellationToken)
    {
        if (_botConfig.DefaultStatus != null)
        {
            _logger.LogInformation("Setting default bot status");
            await _statusService.SetStatusAsync(_botConfig.DefaultStatus.Value);
        }

        if (_botConfig.DefaultActivity != null)
        {
            _logger.LogInformation("Setting default bot activity");
            await _activityService.SetGameAsync(_botConfig.DefaultActivity.Name, _botConfig.DefaultActivity.Type);
        }
    }
}