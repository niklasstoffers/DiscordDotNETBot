using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz.Services.Commands;

internal sealed class CommandHandler : IHostedService
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
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting CommandHandler...");
        await InstallCommandsAsync();
        _client.MessageReceived += HandleCommandAsync;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping CommandHandler...");
        _client.MessageReceived -= HandleCommandAsync;
        return Task.CompletedTask;
    }

    private async Task InstallCommandsAsync()
    {
        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), 
                                        services: _serviceProvider);
    }

    private async Task HandleCommandAsync(SocketMessage message)
    {
        var userMessage = message as SocketUserMessage;
        if (userMessage == null) 
            return;

        int argPos = 0;

        if (!(userMessage.HasCharPrefix('!', ref argPos) || 
            userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            userMessage.Author.IsBot)
            return;

        _logger.LogInformation($"Received \"{message.Content}\" from \"{message.Author.Username}\"");
        var context = new SocketCommandContext(_client, userMessage);

        await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: _serviceProvider);
    }
}