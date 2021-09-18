using System;
using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;
using System.Threading;
using Discord.Commands;
using System.IO;
using Hainz.Config;
using Hainz.Util;
using System.Diagnostics;
using Hainz.Commands.Modules;
using Hainz.Framework;

namespace Hainz
{
    class Program
    {
        private static DiscordSocketClient _client;

        static async Task Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            DiscordSocketClient client = null;
            Exception error = null;
            try
            {
                client = await Startup.Run();
                _client = client;
            }
            catch (Exception ex){ error = ex; }
            if (client == null)
            {
                Console.WriteLine($"Initialization error occured! {error?.Message ?? ""}");
                return;
            }

            using (client)
            {
                try
                {
                    await client.StartAsync();
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("Exception while trying to start client. Internal messages:");
                    ex = ex.Flatten();
                    foreach (var subEx in ex.InnerExceptions)
                        Console.WriteLine(subEx.Message);
                    return;
                }

                // TODO: Add more refined command logic
                bool exit = false;
                while (!exit)
                {
                    string command = Console.ReadLine();
                    if (command == "quit")
                        exit = true;
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Unknown command");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                try
                {
                    await client.StopAsync();
                    MusicPlayer.GetCurrent(null)?.Dispose();
                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("Exception while trying to stop client. Internal messages:");
                    ex = ex.Flatten();
                    foreach (var subEx in ex.InnerExceptions)
                        Console.WriteLine(subEx.Message);
                    return;
                }

                Console.WriteLine("Successfully disconnected. Bye!");
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            _client?.Dispose();
            MusicPlayer.GetCurrent(null)?.Dispose();
        }
    }
}
