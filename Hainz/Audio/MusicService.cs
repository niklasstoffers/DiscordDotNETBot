using Discord.Audio;
using Hainz.Log;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class MusicService : IDisposable
    {
        private IAudioClient _client;
        private Logger _logger;
        private MusicInStream _inputStream;
        private MusicPlayer _player;
        private ConcurrentQueue<Music> _musicQueue;
        private Music _current;
        

        public PlayerState PlayerState { get; private set; }
        public bool CanPlay => Current != null || _musicQueue.Count > 0;
        public Music Current => _current;

        public MusicService(IAudioClient audioClient,
                            Logger logger)
        {
            _client = audioClient;
            _logger = logger;

            _player = new MusicPlayer();
            _musicQueue = new ConcurrentQueue<Music>();
        }

        public void AddToQueue(Music music) =>
            _musicQueue.Enqueue(music);

        public async Task Play()
        {
            if (PlayerState == PlayerState.Playing)
                return;
            else if (PlayerState == PlayerState.Stopped)
            {
                if (!_musicQueue.TryDequeue(out _current))
                    return;

                _inputStream?.Dispose();
                _inputStream = new MusicInStream(_current.Url);
                _player.Input = _inputStream;
            }

            await _player.Start();
            PlayerState = PlayerState.Playing;
        }

        public async Task Stop()
        {
            if (PlayerState == PlayerState.Stopped)
                return;

            await _player.Stop();
            _player.Input = null;
            _inputStream?.Dispose();
            
            PlayerState = PlayerState.Stopped;
        }

        public async Task Pause()
        {
            if (PlayerState != PlayerState.Playing)
                return;

            await _player.Stop();
            PlayerState = PlayerState.Paused;
        }

        public async Task Skip()
        {
            await Stop();
            await Play();
        }

        public async Task Reset()
        {
            await Stop();

            _current = null;
            _musicQueue.Clear();
        }

        public void Dispose()
        {
            Reset().GetAwaiter().GetResult();
            _player.Dispose();
        }
    }
}
