using Hainz.Framework;
using Hainz.IO;
using Hainz.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class MusicInStream : Stream
    {
        private string _url;
        private bool _initialized;
        private bool _disposed;
        private Stream _networkStream;
        private Process _ffmpeg;
        private Task _ffmpegWriter;
        private CancellationTokenSource _ffmpegWriterCTS;
        private AsyncManualResetEvent _needsInputEvent;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }


        public MusicInStream(string musicUrl)
        {
            _url = musicUrl;
        }

        private void Init()
        {
            if (!_initialized)
            {
                _networkStream = new DownloadFileStream(_url);
                _ffmpeg = FFMPEG.CreateToPCMConverter();
                _ffmpegWriterCTS = new CancellationTokenSource();
                _needsInputEvent = new AsyncManualResetEvent(false);

                _ffmpegWriter = Task.Run(async () =>
                {
                    while (!_ffmpegWriterCTS.Token.IsCancellationRequested)
                    {
                        await _needsInputEvent.Wait(_ffmpegWriterCTS.Token);
                        if (_ffmpegWriterCTS.Token.IsCancellationRequested) break;

                        byte[] buffer = new byte[4096];
                        int read = await _networkStream.ReadAsync(buffer, 0, buffer.Length, _ffmpegWriterCTS.Token);
                        await _ffmpeg.StandardInput.BaseStream.WriteAsync(buffer, 0, read, _ffmpegWriterCTS.Token);
                    }
                });
            }

            _initialized = true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Init();

            _needsInputEvent.Set();
            int read = _ffmpeg.StandardOutput.BaseStream.Read(buffer, offset, count);
            _needsInputEvent.Reset();

            return read;
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override void Flush()
        {
            _networkStream?.Flush();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _initialized && !_disposed)
            {
                _ffmpegWriterCTS.Cancel();
                _ffmpegWriter.GetAwaiter().GetResult();

                _networkStream.Dispose();
                _ffmpeg.StandardOutput.Close();
                _ffmpeg.StandardInput.Close();
                _ffmpeg.WaitForExit();
                _ffmpeg.Dispose();

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
