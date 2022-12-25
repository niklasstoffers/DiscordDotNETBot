using Discord.WebSocket;

namespace Hainz.Services;

public interface IGatewayService
{
    Task StartAsync(bool isRestart);
    Task StopAsync();
}