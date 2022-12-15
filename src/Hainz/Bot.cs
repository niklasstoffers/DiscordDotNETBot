using Discord;
using Discord.WebSocket;
using Hainz.Config;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hainz;

internal sealed class Bot : IHostedService
{
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly IHostApplicationLifetime _appLifetime;
    private readonly ILogger<Bot> _logger;

    public Bot(DiscordSocketClient client, 
               BotConfig config,
               IHostApplicationLifetime appLifetime,
               ILogger<Bot> logger) 
    {
        _client = client;
        _config = config;
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
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex, "Exception while trying to start bot");
            _appLifetime.StopApplication();
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        try 
        {
            _logger.LogInformation("Stopping bot...");
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
        catch (Exception ex) 
        {
            _logger.LogCritical(ex, "Exception while trying to stop bot");
        }
    }
}