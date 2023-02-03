using Discord.Commands;
using Hainz.Events.Notifications.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Events;

public class CommandServiceEventPropagatorDecorator
{
    private readonly IMediator _mediator;
    private readonly ILogger<CommandServiceEventPropagatorDecorator> _logger;

    public CommandServiceEventPropagatorDecorator(IMediator mediator, ILogger<CommandServiceEventPropagatorDecorator> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public void Decorate(CommandService commandService)
    {
        _logger.LogInformation("Registering MediatR events to command service");

        commandService.CommandExecuted += async (commandInfo, commandContext, result) => await _mediator.Publish(new CommandExecuted(commandInfo, commandContext, result));
        commandService.Log += async (log) => await _mediator.Publish(new Log(log));

        _logger.LogInformation("MediatR events have been registered to command service");
    }
}