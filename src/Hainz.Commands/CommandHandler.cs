using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.Helpers;
using Hainz.Events.Notifications.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

public sealed class CommandHandler : INotificationHandler<MessageReceived>
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly CommandPrefixResolver _prefixResolver;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(DiscordSocketClient client, 
                          CommandService commands,
                          CommandPrefixResolver prefixResolver,
                          IServiceProvider serviceProvider,
                          ILogger<CommandHandler> logger)
    {
        _client = client;
        _commandService = commands;
        _prefixResolver = prefixResolver;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Handle(MessageReceived notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        _logger.LogTrace("Received message \"{message}\" from \"{user}\"", message.Content, message.Author.Username);

        (SocketUserMessage? commandMessage, int argPos) = await TryParseCommand(message);
        if (commandMessage != null)
        {
            _logger.LogInformation("Received command \"{command}\" from \"{user}\"", message.Content, message.Author.Username);
            var context = new SocketCommandContext(_client, commandMessage);

            try 
            {
                _logger.LogTrace("Executing command");
                await _commandService.ExecuteAsync(
                    context: context, 
                    argPos: argPos,
                    services: _serviceProvider);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Exception during command execution");
            }   
        }
    }

    private async Task<(SocketUserMessage? commandMessage, int argPos)> TryParseCommand(SocketMessage message)
    {
        if (message is SocketUserMessage userMessage)
        {
            int argPos = 0;
            char commandPrefix = await _prefixResolver.GetPrefix(message.Channel);

            if (userMessage.HasCharPrefix(commandPrefix, ref argPos) &&
                !userMessage.Author.IsBot)
            {
                return (userMessage, argPos);
            }
        }

        return (null, 0);
    }
}