using Discord;
using Discord.WebSocket;
using Hainz.Commands;
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
        private CommandHandler _commandHandler;
        private Logger _logger;
        private DiscordLogger _discordLogger;
        private bool _isRunning = false;

        public Bot(BotConfig config,
                   DiscordSocketClient socketClient,
                   CommandHandler commandHandler,
                   Logger logger,
                   DiscordLogger discordLogger)
        {
            _config = config;
            _client = socketClient;
            _commandHandler = commandHandler;
            _logger = logger;
            _discordLogger = discordLogger;
        }

        private async Task Init() 
        {
            await _commandHandler.InstallCommandsAsync();
            _client.Log += _discordLogger.Log;
        }

        public async Task StartAsync()
        {
            if (_isRunning) return;

            await Init();
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            await _client.SetStatusAsync(UserStatus.Online);
            await _client.SetGameAsync(_config.StatusGameName);
            
            _isRunning = true;
        }

        public async Task StopAsync()
        {
            if (!_isRunning) return;

            await _client.LogoutAsync();
            await _client.StopAsync();

            _isRunning = false;
        }

        public void Dispose()
        {
            _client.Dispose();
            _logger.Dispose();
            _discordLogger.Dispose();
        }
    }
}
