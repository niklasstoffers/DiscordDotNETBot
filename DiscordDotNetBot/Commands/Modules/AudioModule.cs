using Discord;
using Discord.Audio;
using Discord.Commands;
using DiscordDotNetBot.API.Youtube;
using DiscordDotNetBot.Config;
using DiscordDotNetBot.Framework;
using DiscordDotNetBot.Tools;
using System;
using System.Threading.Tasks;

namespace DiscordDotNetBot.Commands.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private static IAudioClient _client;
        private static IVoiceChannel _vc;
        private static YoutubeApiClient _ytClient;

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync(params string[] searchInput)
        {
            string search = string.Join(" ", searchInput);
            IVoiceChannel vc = (Context.User as IGuildUser)?.VoiceChannel;

            if (vc == null)
                return;

            if (_vc == null || _vc.Id != vc.Id)
            {
                await DCAsync();

                _vc = vc;
                _client = await vc.ConnectAsync();
            }

            if (string.IsNullOrEmpty(search))
            {
                var musicPlayer = MusicPlayer.GetCurrent(_client);
                await musicPlayer.Play();
            }
            else
            {
                _ytClient ??= new YoutubeApiClient(BotConfig.Current.YoutubeAPIKey);
                string videoUrl = search;
                if (!Uri.TryCreate(search, UriKind.Absolute, out _))
                    videoUrl = await _ytClient.GetMostRelevantForSearch(search);

                if (videoUrl == null)
                    await Context.Channel.SendMessageAsync("Fehler beim Aufrufen der YT Api.");
                else
                {
                    var audioFormats = await YoutubeDl.GetFormats(videoUrl);
                    if (audioFormats == null)
                        await Context.Channel.SendMessageAsync("Fehler beim Extrahieren der Audio Daten.");
                    else
                    {
                        foreach (var format in audioFormats)
                        {
                            if (format.FormatCode == 251)
                            {
                                string url = await YoutubeDl.GetUrl(format.FormatCode, videoUrl, true);
                                var musicPlayer = MusicPlayer.GetCurrent(_client);
                                musicPlayer.AddToQueue(new Music() { Url = url });
                                await musicPlayer.Play();
                            }
                        }
                    }
                }
            }
        }

        [Command("pause", RunMode = RunMode.Async)]
        public async Task PauseAsync()
        {
            if (_client == null)
                return;

            var musicPlayer = MusicPlayer.GetCurrent(_client);
            await musicPlayer.Pause();
        }

        [Command("skip", RunMode = RunMode.Async)]
        public async Task SkipAsync()
        {
            if (_client == null)
                return;

            var musicPlayer = MusicPlayer.GetCurrent(_client);
            await musicPlayer.Skip();
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopAsync()
        {
            if (_client == null)
                return;

            var musicPlayer = MusicPlayer.GetCurrent(_client);
            await musicPlayer.Stop();
        }

        [Command("dc", RunMode = RunMode.Async)]
        public async Task DCAsync()
        {
            try
            {
                if (_client != null) await _client.StopAsync();
                if (_vc != null) await _vc.DisconnectAsync();
            }
            finally
            {
                _client?.Dispose();

                _client = null;
                _vc = null;
            }
        }
    }
}
