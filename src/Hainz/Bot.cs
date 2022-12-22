using Discord;
using Discord.WebSocket;
using Hainz.Commands;
using Hainz.Config;
using Hainz.Services.Discord;
using Hainz.Services.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz;

public sealed class Bot : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly DiscordStatusService _statusService;
    private readonly DiscordActivityService _activityService;
    private readonly CommandHandler _commandHandler;
    private readonly DiscordLogAdapterService _logAdapterService;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<Bot> _logger;

    public Bot(DiscordSocketClient client, 
               BotConfig config,
               DiscordStatusService statusService,
               DiscordActivityService activityService,
               CommandHandler commandHandler,
               DiscordLogAdapterService logAdapterService,
               IHostApplicationLifetime appLifetime,
               ILogger<Bot> logger) 
    {
        _client = client;
        _config = config;
        _statusService = statusService;
        _activityService = activityService;
        _commandHandler = commandHandler;
        _logAdapterService = logAdapterService;
        _appLifetime = appLifetime;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting bot...");

        try 
        {
            // Token validation is done here because although DiscordSocketClient.LoginAsync() also performs token validation
            // it only logs a Warning Message if the supplied token was invalid. However we want to terminate the whole application.
            TokenUtils.ValidateToken(TokenType.Bot, _config.Token);
        }
        catch
        {
            _logger.LogCritical("Supplied bot token was invalid");
            _appLifetime.StopApplication();
            return;
        }

        try 
        {
            await _client.StartAsync();
            await _client.LoginAsync(TokenType.Bot, _config.Token);

            await _commandHandler.StartAsync();
            await _logAdapterService.StartAsync();

            _client.Ready += ClientReady;
            _client.Disconnected += ClientDisconnected;

            _logger.LogInformation("Bot has been started");
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex, "Exception while trying to start bot");
            _appLifetime.StopApplication();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping bot...");
        
        try 
        {
            await _client.LogoutAsync();
            await _client.StopAsync();

            await _commandHandler.StopAsync();
            await _logAdapterService.StopAsync();

            _logger.LogInformation("Bot has been shutdown");
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex, "Exception while trying to stop bot");
        }
    }

    private async Task ClientReady() 
    {
        await _activityService.SetGame(_config.StatusGameName);
        await _statusService.SetStatus(_config.Status);
    }

    private Task ClientDisconnected(Exception disconnectException) 
    {
        _logger.LogCritical(disconnectException, "Disconnected from Discord Gateway");
        return Task.CompletedTask;
    }
}