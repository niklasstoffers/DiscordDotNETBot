using Discord;
using Discord.WebSocket;
using Hainz.Config;
using Hainz.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz
{
    public class Bot : IDisposable
    {
        private BotConfig _config;
        private DiscordSocketClient _client;
        private Logger _logger;

        public Bot(BotConfig config,
                   DiscordSocketClient socketClient,
                   Logger logger)
        {
            _config = config;
            _client = socketClient;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            await _client.StartAsync();
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.SetStatusAsync(UserStatus.Online);
            await _client.SetGameAsync(_config.StatusGameName);
        }

        public async Task StopAsync()
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }

        public void Dispose()
        {
            _client.Dispose();
            _logger.Dispose();
        }
    }
}
