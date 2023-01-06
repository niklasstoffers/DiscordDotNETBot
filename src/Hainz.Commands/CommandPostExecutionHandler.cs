using Discord.Commands;
using Discord.WebSocket;
using Hainz.Events.Notifications.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

public sealed class CommandPostExecutionHandler : INotificationHandler<CommandExecuted>
{
    private readonly ILogger<CommandPostExecutionHandler> _logger;

    public CommandPostExecutionHandler(ILogger<CommandPostExecutionHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(CommandExecuted notification, CancellationToken cancellationToken)
    {
        var result = notification.Result;
        var commandContext = notification.CommandContext;

        if (result.Error != null) 
        {
            if (result.Error != CommandError.UnknownCommand)
                _logger.LogWarning("Command execution failed with error \"{error}\" and reason \"{reason}\"", result.Error, result.ErrorReason);
            else
                _logger.LogInformation("Command \"{content}\" not found", commandContext.Message.Content);

            string? response = result.Error switch 
            {
                CommandError.BadArgCount => $"Invalid command invocation: {result.ErrorReason}",
                CommandError.ParseFailed => $"Failed to parse command: {result.ErrorReason}",
                CommandError.UnmetPrecondition => $"Failed to invoke command: {result.ErrorReason}",
                CommandError.Exception => "Internal exception occured during command execution",
                CommandError.Unsuccessful => "Command execution was unsuccessful",
                CommandError.UnknownCommand => "Unknown command",
                CommandError.MultipleMatches => "Internal error during command parsing",
                CommandError.ObjectNotFound => result.ErrorReason,
                _ => null
            };

            if (response != null)
                await commandContext.Channel.SendMessageAsync(response);
        }
        else
        {
            _logger.LogInformation("Command execution successfull");
        }
    }
}