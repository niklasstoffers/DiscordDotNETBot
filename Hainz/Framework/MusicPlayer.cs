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

namespace Hainz.Framework
{
    public class MusicPlayer : DisposableBase
    {
        private static MusicPlayer _current;
        public static MusicPlayer GetCurrent(IAudioClient audioClient)
        {
            if (audioClient == null)
                return _current;
            _current ??= new MusicPlayer(audioClient);
            _current.SetAudioClient(audioClient);
            return _current;
        }

        private ConcurrentQueue<Music> _playQueue;
        private Music _currentMusic;
        private Stopwatch _currentPlayTime;
        private IAudioClient _client;
        private AudioOutStream _discordAudioStream;
        private Stream _networkInStream;
        private Process _ffmpeg;
        private SemaphoreSlim _stateLock = new SemaphoreSlim(1, 1);
        private Task _playerTask;
        private CancellationTokenSource _playerTaskCTS;

        public MusicPlayerState State { get; private set; }

        private MusicPlayer(IAudioClient client)
        {
            _playQueue = new ConcurrentQueue<Music>();
            _client = client;
            _playerTaskCTS = new CancellationTokenSource();
            State = MusicPlayerState.Stopped;
        }

        public void SetAudioClient(IAudioClient client)
        {
            _client = client;
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
            var player = _playerTask;

            using (var response = await request.GetResponseAsync())
            {
                int audioBufferMS = 2000;
                _networkInStream = response.GetResponseStream();
                _ffmpeg = CreateFFMPEG();
                _discordAudioStream = _client.CreatePCMStream(AudioApplication.Music, bufferMillis: audioBufferMS);
                bool isReading = true;

                var networkInStream = _networkInStream;
                var ffmpeg = _ffmpeg;
                var discordAudioStream = _discordAudioStream;
                var ct = _playerTaskCTS.Token;

                var reader = Task.Run(async () =>
                {
                    long responseLength = response.ContentLength;
                    byte[] buffer = new byte[8192];
                    while (responseLength > 0)
                    {
                        int read = await networkInStream.ReadAsync(buffer, 0, (int)Math.Min(buffer.Length, responseLength));
                        if (read <= 0) break;
                        await ffmpeg.StandardInput.BaseStream.WriteAsync(buffer, 0, read);
                        responseLength -= read;
                        if (ct.IsCancellationRequested) break;
                    }
                    isReading = false;
                });

                var writer = Task.Run(async () =>
                {
                    int wroteBytes = 1;
                    byte[] buffer = new byte[8192];
                    while(isReading || wroteBytes > 0)
                    {
                        var waitTask = Task.Delay(1000);
                        var readTask = ffmpeg.StandardOutput.BaseStream.ReadAsync(buffer, 0, buffer.Length);
                        await Task.WhenAny(waitTask, readTask);
                        
                        if (!isReading && waitTask.IsCompleted)
                            break;
                        wroteBytes = await readTask;

                        while (State == MusicPlayerState.Paused)
                            await Task.Delay(20);
                        if (!isReading && wroteBytes <= 0) break;
                        if (ct.IsCancellationRequested) break;
                        await discordAudioStream.WriteAsync(buffer, 0, wroteBytes);
                    }
                    ;
                });

                var ioTask = Task.WhenAll(reader, writer);
                await ioTask;
                //var cancelTask = new TaskCompletionSource();
                //ct.Register(() => cancelTask.TrySetCanceled(), false);
                //await Task.WhenAny(ioTask, cancelTask.Task);
                await networkInStream.DisposeAsync();
                await discordAudioStream.ClearAsync(CancellationToken.None);
                await discordAudioStream.DisposeAsync();
                ffmpeg.Dispose();
                await Task.Delay(audioBufferMS); // wait one full buffer
            }

            // new player has been started
            if (_playerTask != player)
                return;

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
                Arguments = "-hide_banner -loglevel quiet -i - -c:a pcm_s16le -f s16le -ac 2 -ar 48k pipe:1",
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
                StopInternal();
            }
            finally
            {
                _stateLock.Release();
            }
        }

        private void StopInternal()
        {
            if (State != MusicPlayerState.Stopped)
            {
                _playerTaskCTS.Cancel();
                State = MusicPlayerState.Stopped;
            }
        }

        public void AddToQueue(Music music)
        {
            _playQueue.Enqueue(music);
        }

        public async Task<bool> HasMusic()
        {
            await _stateLock.WaitAsync();
            bool result = false;
            try
            {
                result = _currentMusic != null || _playQueue.Count > 0;
            }
            finally
            {
                _stateLock.Release();
            }
            return result;
        }

        public async Task Skip()
        {
            await _stateLock.WaitAsync();
            try
            {
                if (_currentMusic == null)
                    _playQueue.TryDequeue(out _);

                StopInternal();
                PlayInternal();
            }
            finally
            {
                _stateLock.Release();
            }
        }

        public async Task Clear()
        {
            await _stateLock.WaitAsync();
            try
            {
                _currentMusic = null;
                _playQueue.Clear();
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
