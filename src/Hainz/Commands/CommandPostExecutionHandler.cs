using Discord;
using Discord.Commands;
using Hainz.Services;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

[RequireGatewayConnection(AllowRestart = true)]
public sealed class CommandPostExecutionHandler : IGatewayService
{
    private readonly CommandService _commandService;
    private readonly ILogger<CommandPostExecutionHandler> _logger;

    public CommandPostExecutionHandler(CommandService commandService,
                                       ILogger<CommandPostExecutionHandler> logger)
    {
        _commandService = commandService;
        _logger = logger;
    }

    public Task StartAsync(bool isRestart)
    {
        _logger.LogInformation("Starting command post execution handler");
        _commandService.CommandExecuted += HandleExecutedCommandAsync;
        return Task.CompletedTask;
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping command post execution handler");
        _commandService.CommandExecuted -= HandleExecutedCommandAsync;
        return Task.CompletedTask;
    }

    private async Task HandleExecutedCommandAsync(Optional<CommandInfo> commandInfo, ICommandContext commandContext, IResult result)
    {
        if (result.Error != null) 
        {
            if (result.Error != CommandError.UnknownCommand)
                _logger.LogWarning("Command execution failed with error \"{error}\" and reason \"{reason}\"", result.Error, result.ErrorReason);

            await (result.Error switch 
            {
                CommandError.BadArgCount => commandContext.Channel.SendMessageAsync($"Invalid command invocation: {result.ErrorReason}"),
                CommandError.UnmetPrecondition => commandContext.Channel.SendMessageAsync($"Failed to invoke command: {result.ErrorReason}"),
                _ => Task.CompletedTask
            });
        }
    }
}