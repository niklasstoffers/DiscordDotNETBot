using Discord;
using Discord.Audio;
using Discord.Commands;
using Hainz.API.Youtube;
using Hainz.Config;
using Hainz.Framework;
using Hainz.Tools;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hainz.Commands.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private static IAudioClient _client;
        private static IVoiceChannel _vc;
        private static YoutubeApiClient _ytClient;

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync(params string[] searchInput)
        {
            var stopwatch = Stopwatch.StartNew();
            string search = string.Join(" ", searchInput);
            IVoiceChannel vc = (Context.User as IGuildUser)?.VoiceChannel;

            if (vc == null)
            {
                await Context.Channel.SendMessageAsync("Rosen sind rot, Veilchen sind blau, ohne VC, ist mir das zu ungenau.");
                return;
            }

            var changeVCTask = Task.Run(async () => await ChangeVoiceChannel(vc));

            if (string.IsNullOrEmpty(search))
            {
                var musicPlayer = MusicPlayer.GetCurrent(_client);
                if (musicPlayer != null && await musicPlayer.HasMusic())
                {
                    await changeVCTask;
                    await musicPlayer.Play();
                }
                else
                    await Context.Channel.SendMessageAsync("Soll ich mir ausdenken was du hören möchtest? Schreibs dahinter, du Kek.");
            }
            else
            {
                _ytClient ??= new YoutubeApiClient(BotConfig.Current.YoutubeAPIKey);
                var musicUrlGetterTask = Task.Run(async () => await GetMusicUrl(search));
                await Task.WhenAll(changeVCTask, musicUrlGetterTask);
                string musicUrl = musicUrlGetterTask.Result;

                if (musicUrl == null)
                    await Context.Channel.SendMessageAsync("Fehler beim Extrahieren der Audio Daten. So nen Mist. Jetzt bin ich wütend. Wenn du wissen willst wie wütend ich bin, hier: https://www.youtube.com/watch?v=hveBIv4DB7g");
                else
                {
                    Console.WriteLine($"Getting url execution time: {stopwatch.Elapsed}");
                    var musicPlayer = MusicPlayer.GetCurrent(_client);
                    musicPlayer.AddToQueue(new Music() { Url = musicUrl });
                    await musicPlayer.Play();
                    stopwatch.Stop();
                    Console.WriteLine($"Play command execution time: {stopwatch.Elapsed}");
                }
            }
        }

        private async Task ChangeVoiceChannel(IVoiceChannel vc)
        {
            if (_vc == null || _vc.Id != vc.Id)
            {
                await DCAsync();
                _vc = vc;
                _client = await _vc.ConnectAsync();
            }
        }

        private async Task<string> GetMusicUrl(string search)
        {
            if (!Uri.TryCreate(search, UriKind.Absolute, out _))
                search = await _ytClient.GetMostRelevantForSearch(search);
            if (!string.IsNullOrEmpty(search))
                return await YoutubeDl.GetBestOpusAudio(search);
            return string.Empty;
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
            }
            finally
            {
                _vc = null;
            }
        }
    }
}
