namespace Hainz.Common.Threading;

public sealed class AsyncManualResetEvent
{
    private readonly Task _completed = Task.FromResult(true);
    private readonly Queue<TaskCompletionSource> _waitQueue;
    private readonly object _lock = new();
    private bool _signaled = false;

    public AsyncManualResetEvent(bool initialState)
    {
        _waitQueue = new Queue<TaskCompletionSource>();
        _signaled = initialState;
    }

    public Task WaitAsync(CancellationToken ct = default)
    {
        lock (_lock)
        {
            if (_signaled)
            {
                return _completed;
            }
            else
            {
                var tcs = new TaskCompletionSource();
                _waitQueue.Enqueue(tcs);

                ct.Register(obj => 
                {
                    if (obj is TaskCompletionSource waiter && 
                        !waiter.Task.IsCompleted)
                    {
                        waiter.SetCanceled();
                    }
                }, tcs);

                return tcs.Task;
            }
        }
    }

    public void Set()
    {
        _signaled = true;
        if (_signaled && _waitQueue.Count > 0)
        {
            lock (_lock)
            {
                while (_signaled && _waitQueue.Count > 0)
                {
                    var tcs = _waitQueue.Dequeue();
                    if (!tcs.Task.IsCompleted)
                        tcs.SetResult();
                }
            }
        }
    }

    public void Reset()
    {
        _signaled = false;
    }

    public bool IsSignaled => _signaled;
}