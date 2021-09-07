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
            {
                await Context.Channel.SendMessageAsync("Rosen sind rot, Veilchen sind blau, ohne VC, ist mir das zu ungenau. (Für Analphabeten: Geh in nen VC, du Kek)");
                return;
            }

            if (_vc == null || _vc.Id != vc.Id)
            {
                await DCAsync();
                _vc = vc;
                _client = await _vc.ConnectAsync();
            }

            if (string.IsNullOrEmpty(search))
            {
                var musicPlayer = MusicPlayer.GetCurrent(_client);
                if (musicPlayer != null && await musicPlayer.HasMusic())
                    await musicPlayer.Play();
                else
                    await Context.Channel.SendMessageAsync("Soll ich mir ausdenken was du hören möchtest? Schreibs dahinter, du Kek.");
            }
            else
            {
                _ytClient ??= new YoutubeApiClient(BotConfig.Current.YoutubeAPIKey);
                string videoUrl = search;
                if (!Uri.TryCreate(search, UriKind.Absolute, out _))
                    videoUrl = await _ytClient.GetMostRelevantForSearch(search);

                if (videoUrl == null)
                    await Context.Channel.SendMessageAsync("Fehler beim Aufrufen der YT Api aber an mir kanns nicht liegen. Du bist Schuld.");
                else
                {
                    string bestAudioUrl = await YoutubeDl.GetBestOpusAudio(videoUrl);
                    if (string.IsNullOrEmpty(bestAudioUrl))
                        await Context.Channel.SendMessageAsync("Fehler beim Extrahieren der Audio Daten. So nen Mist. Jetzt bin ich wütend. Wenn du wissen willst wie wütend ich bin, hier: https://www.youtube.com/watch?v=hveBIv4DB7g");
                    else
                    {
                        var musicPlayer = MusicPlayer.GetCurrent(_client);
                        musicPlayer.AddToQueue(new Music() { Url = bestAudioUrl });
                        await musicPlayer.Play();
                    }
                }
            }
        }

        [Command("pause", RunMode = RunMode.Async)]
        public async Task PauseAsync()
        {
            var musicPlayer = MusicPlayer.GetCurrent(_client);
            if (musicPlayer != null)
                await musicPlayer.Pause();
        }

        [Command("skip", RunMode = RunMode.Async)]
        public async Task SkipAsync()
        {
            var musicPlayer = MusicPlayer.GetCurrent(_client);
            if (musicPlayer != null)
                await musicPlayer.Skip();
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopAsync()
        {
            var musicPlayer = MusicPlayer.GetCurrent(_client);
            if (musicPlayer != null)
                await musicPlayer.Stop();
        }

        [Command("clear", RunMode = RunMode.Async)]
        public async Task ClearAsync()
        {
            var musicPlayer = MusicPlayer.GetCurrent(_client);
            if (musicPlayer != null)
            {
                await musicPlayer.Stop();
                await musicPlayer.Clear();
            }
        }

        [Command("dc", RunMode = RunMode.Async)]
        public async Task DCAsync()
        {
            try
            {
                var musicPlayer = MusicPlayer.GetCurrent(_client);
                if (musicPlayer != null)
                {
                    await musicPlayer.Stop();
                    await musicPlayer.Clear();
                }

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
