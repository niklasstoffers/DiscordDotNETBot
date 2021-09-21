using Hainz.IO;
using Hainz.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Audio
{
    public class MusicInStream : Stream
    {
        private string _url;
        private bool _initialized;
        private Stream _networkStream;
        private Process _ffmpeg;

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
            }

            _initialized = true;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Init();

            int read = _networkStream.Read(buffer, offset, count);
            return read;
            _ffmpeg.StandardInput.BaseStream.Write(buffer, offset, read);
            return _ffmpeg.StandardOutput.BaseStream.Read(buffer, offset, count);
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
            if (disposing)
            {
                _networkStream.Dispose();
                _ffmpeg.StandardOutput.Close();
                _ffmpeg.StandardInput.Close();
                _ffmpeg.WaitForExit();
                _ffmpeg.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
