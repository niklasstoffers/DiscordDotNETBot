using Discord.Audio;
using Hainz.Framework;
using Hainz.IO;
using Hainz.Log;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Hainz.Audio
{
    public class MusicPlayer : IDisposable
    {
        private const int MAX_BUFFERS = 50;

        private AudioOutStream _output;
        private IAudioClient _client;
        private Logger _logger;
        private bool _initialized = false;
        private Task _inputReader;
        private Task _outputWriter;
        private BufferBlock<ArraySegment<byte>> _bufferQueue;
        private CancellationTokenSource _cts;
        private ManualResetEventSlim _playEvent;
        private AsyncManualResetEvent _canAppendBuffer;
        private bool _wasStarted = false;
        private bool _disposed = false;

        public MusicInStream Input { get; set; }
        public event EventHandler Finished;

        public MusicPlayer(IAudioClient client,
                           Logger logger)
        {
            _client = client;
            _logger = logger;

            _bufferQueue = new BufferBlock<ArraySegment<byte>>(new DataflowBlockOptions() { BoundedCapacity = MAX_BUFFERS, EnsureOrdered = true });
            _cts = new CancellationTokenSource();
            _playEvent = new ManualResetEventSlim(false);
            _canAppendBuffer = new AsyncManualResetEvent(true);
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
                    byte[] buffer = new byte[8192];
                    read = await Input.ReadAsync(buffer, _cts.Token);
                    await _canAppendBuffer.Wait();
                    _bufferQueue.Post(new ArraySegment<byte>(buffer, 0, read));
                    if (_bufferQueue.Count == MAX_BUFFERS)
                        _canAppendBuffer.Reset();
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
                    var buffer = await _bufferQueue.ReceiveAsync();
                    if (_bufferQueue.Count < MAX_BUFFERS)
                        _canAppendBuffer.Set();
                    read = buffer.Count;
                    await _output.WriteAsync(buffer);
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
                _output?.Dispose();

                _inputReader?.Wait();
                _outputWriter?.Wait();
                _inputReader?.Dispose();
                _outputWriter?.Dispose();

                _cts?.Dispose();
            }
            _disposed = true;
        }
    }
}
