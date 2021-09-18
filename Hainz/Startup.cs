using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Hainz.Commands;
using Hainz.Config;
using Hainz.Log;
using Hainz.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hainz
{
    public static class Startup
    {
        public static async Task<DiscordSocketClient> Run()
        {
            var config = await GetConfig();

            if (config == null)
                return null;

            var client = new DiscordSocketClient(new DiscordSocketConfig() { LogLevel = LogSeverity.Debug });
            var logger = new Logger();

            client.Log += logger.Log;

            var commandService = new CommandService();
            var commandHandler = new CommandHandler(client, commandService);
            await commandHandler.InstallCommandsAsync();

            try
            {
                await client.LoginAsync(TokenType.Bot, config.Token);
                await client.SetGameAsync("dir etwas vor. !play");
            }
            catch
            {
                ConsoleUtil.Write("Faulty token provided!");
                return null;
            }

            return client;
        }

        private static async Task<BotConfig> GetConfig()
        {
            ConsoleUtil.Write("Welcome to the DiscordBot interface!");
            await Task.Delay(2000);
            ConsoleUtil.ClearAndEmptyMessageList();

            BotConfig config = ConfigManager.GetOrCreate();

            if (config == null)
                return null;

            BotConfig.Current = config;

            ConsoleUtil.ClearAndEmptyMessageList();
            ConsoleUtil.Write("Successfully read config!");
            ConsoleUtil.Write("Proceeding to start bot!");
            await Task.Delay(2000);
            ConsoleUtil.ClearAndEmptyMessageList();
            return config;
        }
    }
}
