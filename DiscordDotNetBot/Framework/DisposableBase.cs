using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordDotNetBot.Framework
{
    public abstract class DisposableBase : IDisposable
    {
        protected bool Disposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }
    }
}
