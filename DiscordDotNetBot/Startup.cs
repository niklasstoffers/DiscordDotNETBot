using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordDotNetBot.Commands;
using DiscordDotNetBot.Config;
using DiscordDotNetBot.Log;
using DiscordDotNetBot.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordDotNetBot
{
    public static class Startup
    {
        public static async Task<DiscordSocketClient> Run()
        {
            var config = await GetConfig();

            if (config == null)
                return null;

            var client = new DiscordSocketClient();
            var logger = new Logger();

            client.Log += logger.Log;

            var commandService = new CommandService();
            var commandHandler = new CommandHandler(client, commandService);
            await commandHandler.InstallCommandsAsync();

            try
            {
                await client.LoginAsync(TokenType.Bot, config.Token);
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
