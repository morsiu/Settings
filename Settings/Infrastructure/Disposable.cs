using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSettings.Infrastructure
{
    public abstract class Disposable : IDisposable
    {
        protected bool IsDiposed { get; private set; }

        protected virtual void Dispose(bool isDisposing)
        {
        }

        protected void CheckNotDisposed()
        {
            if (IsDiposed)
            {
                throw new ObjectDisposedException(null);
            }
        }

        public void Dispose()
        {
            if (IsDiposed)
            {
                return;
            }
            Dispose(true);
            IsDiposed = true;
        }
    }
}
