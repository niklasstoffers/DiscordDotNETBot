using Discord;
using Discord.Commands;
using Hainz.Events.Notifications.Commands;
using MediatR;

namespace Hainz.Events.NotificationSources.Commands;

public class CommandExecutedNotificationSource : INotificationSource
{
    private readonly CommandService _commandService;
    private readonly IMediator _mediator;

    public CommandExecutedNotificationSource(CommandService commandService,
                                             IMediator mediator)
    {
        _commandService = commandService;
        _mediator = mediator;
    }

    public Task RegisterAsync()
    {
        _commandService.CommandExecuted += CommandExecutedAsync;
        return Task.CompletedTask;
    }

    public Task DeregisterAsync()
    {
        _commandService.CommandExecuted -= CommandExecutedAsync;
        return Task.CompletedTask;
    }

    private async Task CommandExecutedAsync(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result) => 
        await _mediator.Publish(new CommandExecuted(commandInfo, commandContext, result));
}
