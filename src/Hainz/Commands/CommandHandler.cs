using System.Reflection;
using AutoMapper;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands.TypeReaders;
using Hainz.Services;
using Microsoft.Extensions.Logging;

namespace Hainz.Commands;

[RequireGatewayConnection(AllowRestart = true)]
public sealed class CommandHandler : IGatewayService
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<CommandHandler> _logger;

    public CommandHandler(DiscordSocketClient client, 
                          CommandService commands,
                          IServiceProvider serviceProvider,
                          IMapper mapper,
                          ILogger<CommandHandler> logger)
    {
        _client = client;
        _commandService = commands;
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task StartAsync(bool isRestart)
    {
        _logger.LogInformation("Starting CommandHandler...");

        if (!isRestart)
            await SetupAsync();
            
        _client.MessageReceived += HandleCommandAsync;
    }

    public Task StopAsync()
    {
        _logger.LogInformation("Stopping CommandHandler...");
        _client.MessageReceived -= HandleCommandAsync;
        return Task.CompletedTask;
    }

    private async Task SetupAsync()
    {
        _commandService.AddTypeReader<UserStatus>(new UserStatusTypeReader(_mapper));

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