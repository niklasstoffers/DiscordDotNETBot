using Hainz.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.IO
{
    public class IOBuffer
    {
        private SemaphoreSlim _lock;
        private AsyncManualResetEvent _writeBufferAvailable;
        private AsyncManualResetEvent _readBufferAvailable;
        private AsyncManualResetEvent _bufferReduced;

        private byte[] _buffer;
        private int _bufferSize;
        private int _maxBufferSize;
        private bool _hasWriteBuffer;
        private int _writeBufferPosition;
        private int _readBufferPosition;

        public IOBuffer(int bufferSize, int maxBufferSize)
        {
            if (maxBufferSize < bufferSize)
                throw new ArgumentException(nameof(maxBufferSize), "Max buffer size must be greater or equal to buffer size");
            if (maxBufferSize % bufferSize != 0)
                throw new ArgumentException(nameof(maxBufferSize), "Max buffer size must be a whole-number multiple of buffer size");

            _lock = new SemaphoreSlim(1, 1);
            _writeBufferAvailable = new AsyncManualResetEvent(true);
            _readBufferAvailable = new AsyncManualResetEvent(false);
            _bufferReduced = new AsyncManualResetEvent(false);

            _buffer = new byte[bufferSize];
            _writeBufferPosition = 0;
            _readBufferPosition = 0;

            _bufferSize = bufferSize;
            _maxBufferSize = maxBufferSize;
        }

        public async Task<ArraySegment<byte>> GetWriteSegment(CancellationToken cancelToken = default)
        {
            try
            {
                await _writeBufferAvailable.Wait(cancelToken);
            }
            catch (OperationCanceledException) 
            {
                return default;
            }

            await _lock.WaitAsync();
            try
            {
                if (!_writeBufferAvailable.IsSignaled)
                {
                    _lock.Release();
                    return await GetWriteSegment(cancelToken);
                }

                if (_buffer.Length - _writeBufferPosition <= _bufferSize)
                    EnlargeBuffer();

                int segmentLength = Math.Min(_maxBufferSize - _writeBufferPosition, _bufferSize);
                var segment = new ArraySegment<byte>(_buffer, _writeBufferPosition, segmentLength);
                _hasWriteBuffer = true;

                _writeBufferAvailable.Reset();
                return segment;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task ReleaseWriteSegment(int wroteBytes)
        {
            if (!_hasWriteBuffer) return;

            await _lock.WaitAsync();
            try
            {
                _writeBufferPosition += wroteBytes;
                _hasWriteBuffer = false;

                if (wroteBytes > 0)
                    _readBufferAvailable.Set();
                if (_writeBufferPosition == _maxBufferSize)
                {
#pragma warning disable CS4014
                    _bufferReduced.Wait().ContinueWith(t =>
                    {
                        _writeBufferAvailable.Set();
                    });
#pragma warning restore CS4014
                }
                else
                    _writeBufferAvailable.Set();
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<ArraySegment<byte>> GetReadSegment(CancellationToken cancelToken = default)
        {
            try
            {
                await _readBufferAvailable.Wait(cancelToken);
            }
            catch (OperationCanceledException)
            {
                return default;
            }

            await _lock.WaitAsync();
            try
            {
                if (!_readBufferAvailable.IsSignaled)
                {
                    _lock.Release();
                    return await GetReadSegment(cancelToken);
                }
                int readBufferLength = Math.Min(_writeBufferPosition - _readBufferPosition, _bufferSize);
                var segment = new ArraySegment<byte>(_buffer, _readBufferPosition, readBufferLength);
                ReduceBuffer(_readBufferPosition);

                _readBufferPosition += readBufferLength;
                if (_writeBufferPosition - _readBufferPosition < _bufferSize)
                    _readBufferAvailable.Reset();

                return segment;
            }
            finally
            {
                _lock.Release();
            }
        }

        private void EnlargeBuffer()
        {
            int newBufferLength = Math.Min(_buffer.Length * 2, _maxBufferSize);
            if (newBufferLength == _buffer.Length)
                return;

            byte[] newBuffer = new byte[newBufferLength];
            Array.Copy(_buffer, newBuffer, _buffer.Length);
            _buffer = newBuffer;
        }

        private void ReduceBuffer(int upTo)
        {
            if (_buffer.Length < _maxBufferSize || upTo == 0)
                return;

            byte[] newBuffer = new byte[_buffer.Length - upTo];
            Array.Copy(_buffer, upTo, newBuffer, 0, _buffer.Length - upTo);
            _buffer = newBuffer;

            _readBufferPosition = 0;
            _writeBufferPosition -= upTo;

            _bufferReduced.Set();
        }

    }
}
