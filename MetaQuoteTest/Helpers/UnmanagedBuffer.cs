using MetaQuoteTest.Model;
using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace MetaQuoteTest.Helpers
{
    public class UnmanagedBuffer: CriticalFinalizerObject, IDisposable
    {
        private bool _disposed = false;

        public int Length { get; private set; }

        private IntPtr _ptr;
        public IntPtr Ptr {
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
        
        unsafe public UnmanagedBuffer(Stream stream)
        {
            var length = (int)stream.Length;
            var memIntPtr = Marshal.AllocHGlobal(length);
            var memBytePtr = (byte*)memIntPtr.ToPointer();

            stream.Seek(0, SeekOrigin.Begin);

            using (var ums = new UnmanagedMemoryStream(memBytePtr, length, length, FileAccess.Write))
            {
                stream.CopyTo(ums);
            }               

            // Marshal.FreeHGlobal(memIntPtr);
            Ptr = memIntPtr;
            Length = length;
        }

        public void Dispose()
        {
            if (_disposed)
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
    [StructLayout(LayoutKind.Explicit, Size = 60)]
    public unsafe struct TestUnsafeStruct
    {
        [FieldOffset(GeobaseOffsets.Header.Version)]
        int _version;
        [FieldOffset(GeobaseOffsets.Header.Name)]
        fixed sbyte _name[32];
        public int Verision => _version;
        public string Name
        {
            get
            {
                fixed (sbyte* namePtr = _name)
                {
                    return new string((char*)namePtr);
                }
            }
        }
    }
}
