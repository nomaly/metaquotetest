﻿using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace GeobaseModel.Helpers
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

        public UnmanagedBuffer(IntPtr ptr)
        {
            Ptr = ptr;
        }

        public static UnmanagedBuffer FromFile(string path)
            => new UnmanagedBuffer(Utils.LoadFile(path));

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
