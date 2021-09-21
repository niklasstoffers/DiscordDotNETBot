using Hainz.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.Tests.IO
{
    [TestClass]
    public class IOBufferTests
    {
        [TestMethod]
        public async Task FullReadWriteSyncTest()
        {
            int bufferSize = 4096, maxBufferSize = 8192;
            IOBuffer buffer = new IOBuffer(bufferSize, maxBufferSize);
            byte[] data = GenerateData(maxBufferSize);

            for (int dataWritten = 0; dataWritten < data.Length;)
            {
                var writeBuffer = await buffer.GetWriteSegment();
                CopyBufferToSegment(data, dataWritten, writeBuffer);
                dataWritten += writeBuffer.Count;
                await buffer.ReleaseWriteSegment(writeBuffer.Count);
            }

            byte[] read = new byte[maxBufferSize];
            for(int dataRead = 0; dataRead < maxBufferSize;)
            {
                var readBuffer = await buffer.GetReadSegment();
                CopySegmentToBuffer(readBuffer, read, dataRead);
                dataRead += readBuffer.Count;
            }

            bool areEqual = AreEqual(data, read);
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public async Task MessageQueueSyncTest()
        {
            int bufferSize = 4096, maxBufferSize = 8192;
            IOBuffer buffer = new IOBuffer(bufferSize, maxBufferSize);
            int messages = 10;
            bool wrongMessage = false;

            for(int i = 0; i < messages; i++)
            {
                byte[] message = GenerateData(bufferSize);
                var writeSegment = await buffer.GetWriteSegment();
                CopyBufferToSegment(message, 0, writeSegment);
                await buffer.ReleaseWriteSegment(writeSegment.Count);

                var readSegment = await buffer.GetReadSegment();
                byte[] received = new byte[bufferSize];
                CopySegmentToBuffer(readSegment, received, 0);

                wrongMessage = wrongMessage || !AreEqual(message, received);
            }

            Assert.IsFalse(wrongMessage);
        }

        [TestMethod]
        public async Task MessageQueueAsyncTest()
        {
            int bufferSize = 4096, maxBufferSize = bufferSize * 32;
            IOBuffer buffer = new IOBuffer(bufferSize, maxBufferSize);
            byte[] data = GenerateData(maxBufferSize * 2);
            byte[] read = new byte[maxBufferSize * 2];

            var writer = Task.Run(async () =>
            {
                var random = new Random();
                for(int i = 0; i < data.Length;)
                {
                    int delay = random.Next(0, 50);

                    var writeBuffer = await buffer.GetWriteSegment();
                    CopyBufferToSegment(data, i, writeBuffer);
                    await buffer.ReleaseWriteSegment(writeBuffer.Count);
                    i += writeBuffer.Count;

                    await Task.Delay(delay);
                }
            });

            var reader = Task.Run(async () =>
            {
                for(int i = 0; i < read.Length;)
                {
                    var readSegment = await buffer.GetReadSegment();
                    CopySegmentToBuffer(readSegment, read, i);
                    i += readSegment.Count;
                }
            });

            await Task.WhenAll(writer, reader);
            int[] faultyIndices = data.Select((x, i) => (x, i)).Where(m => m.x != read[m.i]).Select(m => m.i).ToArray();
            Assert.IsTrue(AreEqual(data, read));
        }

        private byte[] GenerateData(int length)
        {
            var random = new Random();
            return Enumerable.Range(0, length)
                             .Select(x => (byte)random.Next(byte.MinValue, byte.MaxValue))
                             .ToArray();
        }

        private bool AreEqual(byte[] buffer1, byte[] buffer2)
        {
            if (buffer1.Length != buffer2.Length)
                return false;

            return Enumerable.Range(0, buffer1.Length).All(x => buffer1[x] == buffer2[x]);
        }

        private void CopyBufferToSegment(byte[] buffer, int bufferOffset, ArraySegment<byte> segment)
        {
            for (int i = bufferOffset; i < Math.Min(bufferOffset + segment.Count, buffer.Length); i++)
                segment[i - bufferOffset] = buffer[i];
        }

        public void CopySegmentToBuffer(ArraySegment<byte> segment, byte[] buffer, int bufferOffset)
        {
            for (int i = bufferOffset; i < Math.Min(bufferOffset + segment.Count, buffer.Length); i++)
                buffer[i] = segment[i - bufferOffset];
        }
    }
}
