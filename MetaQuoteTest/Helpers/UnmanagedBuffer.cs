using MetaQuoteTest.Model;
using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;

namespace MetaQuoteTest.Helpers
{
    public class UnmanagedBuffer : CriticalFinalizerObject, IDisposable
    {
        private bool _disposed = false;
        
        private IntPtr _ptr;
        public IntPtr Ptr
        {
            get
            {
                if (!_disposed)
                    return _ptr;
                throw new Exception("Already disposed");
            }
            private set
            {
                _ptr = value;
            }
        }

        public UnmanagedBuffer(string path)
        {
            Ptr = Utils.LoadFile(path);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Marshal.FreeHGlobal(Ptr);
            }

            _disposed = true;
        }

        ~UnmanagedBuffer()
        {
            Dispose();
        }
    }
}
