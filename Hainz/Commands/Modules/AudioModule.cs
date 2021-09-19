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
        private VoiceChannelService _vcService;
        private MusicService _musicService;
        private MusicBuilder _musicBuilder;
        private Logger _logger;

        public AudioModule(VoiceChannelService vcService,
                              MusicService musicService,
                              MusicBuilder musicBuilder,
                              Logger logger)
        {
            _vcService = vcService;
            _musicService = musicService;
            _musicBuilder = musicBuilder;
            _logger = logger;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync()
        {
            await JoinAsync();

            if (!_vcService.IsConnected)
                return;

            if (_musicService.CanPlay)
                await _musicService.Play();
            else
                await Context.Channel.SendMessageAsync("Was soll gespielt werden?");
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayAsync(params string[] search)
        {
            await JoinAsync();

            if (!_vcService.IsConnected)
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

            string message = $"Spielt nun {music.Title} [{music.Length}:c]";
            if (_musicService.CanPlay)
                message = $"{music.Title} wurde zur Warteschlange hinzugefügt";

            _musicService.AddToQueue(music);
            await _musicService.Play();
            
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
                await _vcService.ConnectAsync(vc);

            if (!_vcService.IsConnected)
                await Context.Channel.SendMessageAsync("Fehler beim Beitreten zum VC. Möglicherweise fehlen Berechtigungen zum Zutritt.");
        }

        [Command("skip", RunMode = RunMode.Async)]
        public async Task SkipAsync()
        {
            await _musicService.Skip();
        }

        [Command("pause", RunMode = RunMode.Async)]
        public async Task PauseAsync()
        {
            await _musicService.Pause();
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task StopAsync()
        {
            await _musicService.Stop();
        }

        [Command("clear", RunMode = RunMode.Async)]
        public async Task ClearAsync()
        {
            await _musicService.Stop();
            await _musicService.Reset();
        }

        [Command("dc", RunMode = RunMode.Async)]
        public async Task DCAsync()
        {
            await _musicService.Stop();
            await _musicService.Reset();
            await _vcService.DisconnectAsync();
        }
    }
}
