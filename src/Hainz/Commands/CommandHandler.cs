using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

public sealed class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(DiscordSocketClient client, 
                          CommandService commands,
                          IServiceProvider serviceProvider,
                          ILogger<CommandHandler> logger)
    {
        _commands = commands;
        _client = client;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task StartAsync()
    {
        _logger.LogInformation("Starting CommandHandler...");
        await InstallCommandsAsync();
        _client.MessageReceived += HandleCommandAsync;
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping CommandHandler...");
        _client.MessageReceived -= HandleCommandAsync;
        return Task.CompletedTask;
    }

    private async Task InstallCommandsAsync()
    {
        _logger.LogTrace("Installing Command Modules");
        await _commands.AddModulesAsync(assembly: Assembly.GetExecutingAssembly(), 
                                        services: _serviceProvider);
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        if (message is not SocketUserMessage userMessage)
            return;

        int argPos = 0;

        if (!(userMessage.HasCharPrefix('!', ref argPos) || 
            userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            userMessage.Author.IsBot)
            return;

        _logger.LogInformation("Received command \"{command}\" from \"{user}\"", message.Content, message.Author.Username);
        var context = new SocketCommandContext(_client, userMessage);

        await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: _serviceProvider);
    }
}