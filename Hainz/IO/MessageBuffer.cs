using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Hainz.IO 
{
    public class MessageBuffer : OutputBase, IInput
    {
        private BufferBlock<string> _messageQueue;

        public MessageBuffer() 
        {
            _messageQueue = new BufferBlock<string>();
        }

        public override Task AppendAsync(string message)
        {
            _messageQueue.Post(message);
            return Task.CompletedTask;
        }

        public async Task<string> ReceiveNext(CancellationToken ct) =>
            await _messageQueue.ReceiveAsync<string>();
    }
}