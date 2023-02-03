using Discord.WebSocket;
using Hainz.Events.Notifications.Connection;
using Hainz.Events.Notifications.Logs;
using Hainz.Events.Notifications.Messages;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Hainz.Events;

public class ClientEventPropagatorDecorator
{
    private readonly IMediator _mediator;
    private readonly ILogger<ClientEventPropagatorDecorator> _logger;

    public ClientEventPropagatorDecorator(IMediator mediator, ILogger<ClientEventPropagatorDecorator> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public void Decorate(DiscordSocketClient client)
    {
        _logger.LogInformation("Registering MediatR events to client");

        client.Connected += async () => await _mediator.Publish(new Connected());
        client.Disconnected += async (exception) => await _mediator.Publish(new Disconnected(exception));
        client.Ready += async () => await _mediator.Publish(new Ready());

        client.MessageReceived += async (message) => await _mediator.Publish(new MessageReceived(message));

        client.Log += async (log) => await _mediator.Publish(new Log(log));

        _logger.LogInformation("MediatR events have been registered");
    }
}