using Discord.Audio;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordDotNetBot.Framework
{
    public class MusicPlayer : DisposableBase
    {
        private static MusicPlayer _current;
        public static MusicPlayer GetCurrent(IAudioClient audioClient)
        {
            if (audioClient == null)
                return _current;
            return _current ??= new MusicPlayer(audioClient);
        }

        private ConcurrentQueue<Music> _playQueue;
        private Music _currentMusic;
        private Stopwatch _currentPlayTime;
        private IAudioClient _client;
        private Stream _discordAudioStream;
        private Stream _networkInStream;
        private Process _ffmpeg;
        private SemaphoreSlim _stateLock = new SemaphoreSlim(1, 1);
        private Task _playerTask;

        public MusicPlayerState State { get; private set; }

        private MusicPlayer(IAudioClient client)
        {
            _playQueue = new ConcurrentQueue<Music>();
            _client = client;
            State = MusicPlayerState.Stopped;
        }

        public async Task Play()
        {
            await _stateLock.WaitAsync();
            try
            {
                PlayInternal();
            }
            finally
            {
                _stateLock.Release();
            }
        }

        private void PlayInternal()
        {
            if (State == MusicPlayerState.Playing)
                return;
            else if (State == MusicPlayerState.Paused && _currentMusic != null)
            {
                State = MusicPlayerState.Playing;
                _currentPlayTime.Start();
            }
            else if (_playQueue.Count > 0)
            {
                _currentPlayTime = Stopwatch.StartNew();
                State = MusicPlayerState.Playing;
                _playQueue.TryDequeue(out _currentMusic);
                _playerTask = Task.Run(Player);
            }
        }

        private async Task Player()
        {
            Music current = _currentMusic;
            var request = MakeHttpRequest(current);

            using (var response = await request.GetResponseAsync())
            {
                _networkInStream = response.GetResponseStream();
                _ffmpeg = CreateFFMPEG();
                _discordAudioStream ??= _client.CreatePCMStream(AudioApplication.Music);
                bool isReading = true;

                var reader = Task.Run(async () =>
                {
                    long responseLength = response.ContentLength;
                    byte[] buffer = new byte[8192];
                    while (responseLength > 0)
                    {
                        int read = await _networkInStream.ReadAsync(buffer, 0, (int)Math.Min(buffer.Length, responseLength));
                        if (read <= 0) break;
                        await _ffmpeg.StandardInput.BaseStream.WriteAsync(buffer, 0, read);
                        responseLength -= read;
                    }
                    isReading = false;
                });

                var writer = Task.Run(async () =>
                {
                    int wrote = 1;
                    byte[] buffer = new byte[8192];
                    while(isReading || wrote > 0)
                    {
                        wrote = await _ffmpeg.StandardOutput.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                        while (State == MusicPlayerState.Paused)
                            await Task.Delay(20);
                        if (wrote <= 0) break;
                        await _discordAudioStream.WriteAsync(buffer, 0, wrote);
                    }
                });

                await Task.WhenAll(reader, writer);
                await _networkInStream.DisposeAsync();
                _ffmpeg.Dispose();
            }

            await _stateLock.WaitAsync();
            try
            {
                _currentMusic = null;
                _currentPlayTime.Stop();
                State = MusicPlayerState.Stopped;
            }
            finally
            {
                _stateLock.Release();
            }

            await Play();
        }

        private HttpWebRequest MakeHttpRequest(Music music)
        {
            var httpRequest = WebRequest.CreateHttp(music.Url);
            httpRequest.Method = HttpMethod.Get.ToString();
            return httpRequest;
        }

        private Process CreateFFMPEG()
        {
            return Process.Start(new ProcessStartInfo()
            {
                FileName = "ffmpeg",
                Arguments = "-hide_banner -loglevel quiet -f webm -i - -c:a pcm_s16le -f s16le -ac 2 -ar 48k pipe:1",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            });
        }

        public async Task Pause()
        {
            await _stateLock.WaitAsync();
            try
            {
                if (State == MusicPlayerState.Playing)
                {
                    State = MusicPlayerState.Paused;
                    _currentPlayTime.Stop();
                }
            }
            finally
            {
                _stateLock.Release();
            }
        }

        public async Task Stop()
        {
            await _stateLock.WaitAsync();
            try
            {
                await StopInternal();
            }
            finally
            {
                _stateLock.Release();
            }
        }

        private async Task StopInternal()
        {
            if (State != MusicPlayerState.Stopped)
            {
                _ffmpeg?.Dispose();
                if (_networkInStream != null)
                    await _networkInStream.DisposeAsync();
                _currentMusic = null;
                _currentPlayTime.Stop();
                _currentPlayTime = null;
                State = MusicPlayerState.Stopped;
            }
        }

        public void AddToQueue(Music music)
        {
            _playQueue.Enqueue(music);
        }

        public async Task Skip()
        {
            await _stateLock.WaitAsync();
            try
            {
                if (_currentMusic == null)
                    _playQueue.TryDequeue(out _);

                await StopInternal();
                PlayInternal();
            }
            finally
            {
                _stateLock.Release();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!Disposed && disposing)
            {
                try
                {
                    _client?.Dispose();
                    _discordAudioStream?.Dispose();
                    _discordAudioStream?.Dispose();
                    _networkInStream?.Dispose();
                    _ffmpeg?.Dispose();
                }
                catch
                {

                }
            }
            base.Dispose(disposing);
        }
    }
}
