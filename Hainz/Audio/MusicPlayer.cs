using Discord.Audio;
using Hainz.IO;
using Hainz.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class MusicPlayer : IDisposable
    {
        private const int DEFAULT_BUFFER_SIZE = 4096; // 4kb
        private const int MAX_BUFFER_SIZE = 512_000; // 0.5 MB

        private AudioOutStream _output;
        private IAudioClient _client;
        private Logger _logger;
        private bool _initialized = false;
        private Task _inputReader;
        private Task _outputWriter;
        private CancellationTokenSource _cts;
        private ManualResetEventSlim _playEvent;
        private bool _wasStarted = false;
        private IOBuffer _buffer;
        private bool _disposed = false;

        public MusicInStream Input { get; set; }
        public event EventHandler Finished;

        public MusicPlayer(IAudioClient client,
                           Logger logger)
        {
            _client = client;
            _logger = logger;

            _cts = new CancellationTokenSource();
            _buffer = new IOBuffer(DEFAULT_BUFFER_SIZE, MAX_BUFFER_SIZE);
            _playEvent = new ManualResetEventSlim(false);
        }

        private void Init()
        {
            if (!_initialized)
            {
                _output = _client.CreatePCMStream(AudioApplication.Music, bufferMillis: 2000);
                _inputReader = new Task(async () => await Reader());
                _outputWriter = new Task(async () => await Writer());
            }
            _initialized = true;
        }

        public void Start()
        {
            Init();

            if (!_wasStarted)
            {
                _inputReader.Start();
                _outputWriter.Start();
                _wasStarted = true;
            }

            _playEvent.Set();
        }

        public void Stop()
        {
            _playEvent.Reset();
        }

        private async Task Reader()
        {
            int read = 1;
            while (read > 0 && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    _playEvent.Wait(_cts.Token);
                    byte[] writeBuffer = await _buffer.GetWriteSegment(_cts.Token);
                    read = await Input.ReadAsync(writeBuffer, _cts.Token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    await _logger.LogMessageAsync("Exception while reading from music input stream", LogLevel.Error);
                    await _logger.LogExceptionAsync(ex);
                    break;
                }
            }
        }

        private async Task Writer()
        {
            int read = 1;
            while (read > 0 && !_cts.Token.IsCancellationRequested)
            {
                try
                {
                    _playEvent.Wait(_cts.Token);
                    byte[] readBuffer = await _buffer.GetReadSegment(_cts.Token);
                    read = readBuffer.Length;
                    await _output.WriteAsync(readBuffer);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    await _logger.LogMessageAsync("Exception while writing to music output stream", LogLevel.Error);
                    await _logger.LogExceptionAsync(ex);
                    break;
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _cts.Cancel();

                _playEvent.Reset();
                _output.Dispose();

                _inputReader.GetAwaiter().GetResult();
                _outputWriter.GetAwaiter().GetResult();
            }
            _disposed = true;
        }
    }
}
