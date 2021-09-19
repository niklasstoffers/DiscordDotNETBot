using Hainz.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hainz.IO
{
    public abstract class OutputBase : DisposableBase, IOutput
    {
        private TaskCompletionSource _contextExited;
        private SemaphoreSlim _contextLock = new SemaphoreSlim(1, 1);

        public OutputContextType CurrentContextType { get; private set; }

        public OutputBase()
        {
            _contextExited = new TaskCompletionSource();
            _contextExited.SetResult();
        }

        public abstract Task AppendAsync(string message);

        public async Task<OutputContext> BeginContextAsync(OutputContextType contextType)
        {
            while (true)
            {
                await _contextExited.Task;
                await _contextLock.WaitAsync();
                try
                {
                    if (!_contextExited.Task.IsCompleted) // another thread already entered a new OutputContext
                        continue;

                    _contextExited = new TaskCompletionSource();
                    var context = new OutputContext(() =>
                    {
                        CurrentContextType = OutputContextType.Default;
                        _contextExited.SetResult();
                    });
                    return context;
                }
                finally
                {
                    _contextLock.Release();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
