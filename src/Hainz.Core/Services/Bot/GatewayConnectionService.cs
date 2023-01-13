using Discord;
using Discord.WebSocket;
using Hainz.Core.Config;
using Hainz.Core.Events;
using Hainz.Hosting;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Core.Services.Bot;

public sealed class GatewayConnectionService : GatewayServiceBase
{
    private readonly DiscordSocketClient _client;
    private readonly BotConfig _config;
    private readonly IMediator _mediator;
    private readonly ILogger<GatewayConnectionService> _logger;

    public GatewayConnectionService(DiscordSocketClient client,
                                    BotConfig config,
                                    IMediator mediator,
                                    ILogger<GatewayConnectionService> logger)
    {
        _client = client;
        _config = config;
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task StartAsync()
    {
        try
        {
            _logger.LogInformation("Trying to establish gateway connection");

            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
        }
        catch (Exception ex)
        {
            await _mediator.Publish(new ApplicationShutdownRequest());
            _logger.LogCritical(ex, "Error while trying to establish gateway connection");
        }
    }

    public override async Task StopAsync()
    {
        try
        {
            _logger.LogInformation("Trying to shutdown gateway connection");

            await _client.LogoutAsync();
            await _client.StopAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Error while trying to shutdown gateway connection");
        }
    }
}