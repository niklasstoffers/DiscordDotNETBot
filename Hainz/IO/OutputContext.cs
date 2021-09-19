using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hainz.IO
{
    public class OutputContext : IDisposable
    {
        private bool _disposed = false;
        private Action _callback;

        public OutputContext(Action callback)
        {
            _callback = callback;
        }

        public void Dispose()
        {
            if (!_disposed)
                _callback?.Invoke();
            _disposed = true;
        }
    }
}
