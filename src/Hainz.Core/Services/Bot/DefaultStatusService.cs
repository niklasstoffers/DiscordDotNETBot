using Hainz.Core.Config;
using Hainz.Core.Services.Status;
using Hainz.Data.Queries.Bot.DefaultActivity;
using Hainz.Data.Queries.Bot.DefaultStatus;
using Hainz.Events.Notifications.Connection;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Bot;

public sealed class DefaultStatusService : INotificationHandler<Ready>
{
    private readonly StatusService _statusService;
    private readonly ActivityService _activityService;
    private readonly IMediator _mediator;
    private readonly ILogger<DefaultStatusService> _logger;

    public DefaultStatusService(StatusService statusService,
                                ActivityService activityService,
                                IMediator mediator,
                                ILogger<DefaultStatusService> logger)
    {
        _statusService = statusService;
        _activityService = activityService;
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(Ready notification, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Setting default activity");
            var defaultActivity = await _mediator.Send(new GetDefaultActivityQuery(), cancellationToken);
            await _activityService.SetGameAsync(defaultActivity.Name, defaultActivity.Type);    
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while setting default activity");
        }

        try
        {
            _logger.LogInformation("Setting default status");
            var defaultStatus = await _mediator.Send(new GetDefaultStatusQuery(), cancellationToken);
            await _statusService.SetStatusAsync(defaultStatus);
        }       
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception while setting default status");
        }
    }
}