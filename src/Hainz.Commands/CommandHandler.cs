using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.TypeReaders;
using Hainz.Core.Services;
using Hainz.Events.Notifications.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

public sealed class CommandHandler : INotificationHandler<MessageReceived>, IGatewayService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IEnumerable<TypeReaderBase> _typeReaders;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(DiscordSocketClient client, 
                          CommandService commands,
                          IEnumerable<TypeReaderBase> typeReaders,
                          IServiceProvider serviceProvider,
                          ILogger<CommandHandler> logger)
    {
        _client = client;
        _commandService = commands;
        _typeReaders = typeReaders;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task StartAsync(bool isRestart)
    {
        _logger.LogInformation("Starting CommandHandler...");

        if (!isRestart)
            await SetupAsync();
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping CommandHandler...");
        return Task.CompletedTask;
    }

    private async Task SetupAsync()
    {
        _logger.LogTrace("Adding type readers");
        AddTypeReaders();

        try 
        {
            _logger.LogTrace("Installing Command Modules");
            await _commandService.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), 
                                                  services: _serviceProvider);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error while trying to install commands");
        }
    }

    private void AddTypeReaders()
    {
        foreach (var typeReader in _typeReaders)
        {
            _commandService.AddTypeReader(typeReader.ForType, typeReader);
        }
    }

    public async Task Handle(MessageReceived notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        if (message is not SocketUserMessage userMessage)
            return;

        int argPos = 0;

        if (!(userMessage.HasCharPrefix('!', ref argPos) || 
            userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            userMessage.Author.IsBot)
            return;

        _logger.LogInformation("Received command \"{command}\" from \"{user}\"", message.Content, message.Author.Username);
        var context = new SocketCommandContext(_client, userMessage);

        try 
        {
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