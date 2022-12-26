using Discord;
using Discord.WebSocket;
using Hainz.Config.Bot;
using Hainz.Services.Discord;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz;

public sealed class Bot : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly DiscordStatusService _statusService;
    private readonly DiscordActivityService _activityService;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<Bot> _logger;

    public Bot(DiscordSocketClient client, 
               BotConfig config,
               DiscordStatusService statusService,
               DiscordActivityService activityService,
               IHostApplicationLifetime appLifetime,
               ILogger<Bot> logger) 
    {
        _client = client;
        _config = config;
        _statusService = statusService;
        _activityService = activityService;
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

            _client.Ready += ClientReadyAsync;
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

            _logger.LogInformation("Bot has been shutdown");
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex, "Exception while trying to stop bot");
        }
    }

    private async Task ClientReadyAsync() 
    {
        if (_config.DefaultActivity != null) await _activityService.SetGameAsync(_config.DefaultActivity.Name, _config.DefaultActivity.Type);
        if (_config.DefaultStatus != null) await _statusService.SetStatusAsync(_config.DefaultStatus.Value);
    }

    private Task ClientDisconnected(Exception disconnectException) 
    {
        _logger.LogCritical(disconnectException, "Disconnected from Discord Gateway");
        return Task.CompletedTask;
    }
}