using Discord;
using Discord.Audio;
using Discord.Commands;
using Hainz.API.Youtube;
using Hainz.Audio;
using Hainz.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Commands.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private AudioManager _audioManager;
        private MusicBuilder _musicBuilder;
        private Logger _logger;

        private VoiceChannelService VCService => _audioManager.VCService;
        private MusicService MusicService => _audioManager.MusicService;

        public AudioModule(AudioManager audioManager,
                           MusicBuilder musicBuilder,
                           Logger logger)
        {
            _audioManager = audioManager;
            _musicBuilder = musicBuilder;
            _logger = logger;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync()
        {
            await JoinAsync();

            if (!VCService.IsConnected)
                return;

            if (MusicService.CanPlay)
                MusicService.Play();
            else
                await Context.Channel.SendMessageAsync("Was soll gespielt werden?");
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync(params string[] search)
        {
            await JoinAsync();

            if (!VCService.IsConnected)
                return;

            string searchQuery = string.Join(" ", search);
            Music music = await _musicBuilder.WithQuery(searchQuery)
                                             .WithPlatform(MusicPlatform.Youtube)
                                             .BuildAsync();

            if (music == null)
            {
                await Context.Channel.SendMessageAsync("Fehler beim Abruf der Audiodateien");
                return;
            }

            string message = $@"Spielt nun ""{music.Title}"" von ""{music.Artist}"" [{music.Length:c}]";
            if (MusicService.CanPlay)
                message = $"{music.Title} wurde zur Warteschlange hinzugefügt";

            MusicService.AddToQueue(music);
            MusicService.Play();
            
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinAsync()
        {
            var user = Context.User as IGuildUser;
            if (user == null)
                return;

            IVoiceChannel vc = user.VoiceChannel;
            if (vc == null)
                await Context.Channel.SendMessageAsync("Du musst in einem VC sein um diesen Command zu benutzen.");
            else
                await VCService.ConnectAsync(vc);

            if (!VCService.IsConnected)
                await Context.Channel.SendMessageAsync("Fehler beim Beitreten zum VC. Möglicherweise fehlen Berechtigungen zum Zutritt.");
        }

        [Command("skip", RunMode = RunMode.Async)]
        public Task SkipAsync()
        {
            MusicService?.Skip();
            return Task.CompletedTask;
        }

        [Command("pause", RunMode = RunMode.Async)]
        public Task PauseAsync()
        {
            MusicService?.Pause();
            return Task.CompletedTask;
        }

        [Command("stop", RunMode = RunMode.Async)]
        public Task StopAsync()
        {
            MusicService?.Stop();
            return Task.CompletedTask;
        }

        [Command("clear", RunMode = RunMode.Async)]
        public Task ClearAsync()
        {
            MusicService?.Stop();
            MusicService?.Reset();
            return Task.CompletedTask;
        }

        [Command("dc", RunMode = RunMode.Async)]
        public async Task DCAsync()
        {
            MusicService?.Stop();
            MusicService?.Reset();
            await VCService.DisconnectAsync();
        }
    }
}
