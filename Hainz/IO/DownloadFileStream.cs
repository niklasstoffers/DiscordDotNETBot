using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.IO
{
    public class DownloadFileStream : Stream
    {
        private string _downloadUrl;
        private bool _initialized = false;
        private HttpWebRequest _request;
        private WebResponse _response;
        private Stream _responseStream;

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public DownloadFileStream(string url)
        {
            _downloadUrl = url;
        }

        private void Init()
        {
            if (!_initialized)
            {
                _request = WebRequest.CreateHttp(_downloadUrl);
                _request.Method = HttpMethod.Get.ToString();
                _response = _request.GetResponse();
                _responseStream = _response.GetResponseStream();
            }
            _initialized = true;
        }

        public override void Flush()
        {
            _responseStream?.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Init();
            return _responseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _responseStream?.Dispose();
                _response?.Dispose();

                _request = null;
                _responseStream = null;
                _response = null;
            }
            base.Dispose(disposing);
        }
    }
}
