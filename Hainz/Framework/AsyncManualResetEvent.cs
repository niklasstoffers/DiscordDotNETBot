using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.Framework
{
    public class AsyncManualResetEvent
    {
        private Task _completed = Task.FromResult(true);
        private Queue<TaskCompletionSource> _waitQueue;
        private object _lock = new object();
        private bool _signaled = false;

        public AsyncManualResetEvent(bool initialState)
        {
            _waitQueue = new Queue<TaskCompletionSource>();
            _signaled = initialState;
        }

        public Task Wait(CancellationToken ct = default)
        {
            lock (_lock)
            {
                if (_signaled)
                {
                    _signaled = false;
                    return _completed;
                }
                else
                {
                    var tcs = new TaskCompletionSource();
                    _waitQueue.Enqueue(tcs);
                    ct.Register(() => tcs.SetCanceled());
                    return tcs.Task;
                }
            }
        }

        public void Set()
        {
            _signaled = true;
            while (_signaled && _waitQueue.Count > 0)
            {
                lock (_lock)
                {
                    if (_signaled && _waitQueue.Count > 0)
                    {
                        var tcs = _waitQueue.Dequeue();
                        tcs.SetResult();
                    }
                }
            }
        }

        public void Reset()
        {
            _signaled = false;
        }
    }
}
