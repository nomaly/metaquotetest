using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace GeobaseModel.Helpers
{
    public static class Utils
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadFile(
            SafeFileHandle hFile,                           // Used since CreateFile returns SafeFileHandle
            IntPtr pBuffer,                                 // Can be byte[], but for our purposes, it's easier to read to a IntPtr
            uint NumberOfBytesToRead,                       // Shouldn't be int because this isn't going to be negative
            out uint pNumberOfBytesRead,                    // Could be IntPtr but easier to just use this
            [In] ref NativeOverlapped lpOverlapped			// Can also be IntPtr
        );

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern SafeFileHandle CreateFile(
                string lpFileName,
                [MarshalAs(UnmanagedType.U4)] FileAccess dwDesiredAccess,
                [MarshalAs(UnmanagedType.U4)] FileShare dwShareMode,
                IntPtr lpSecurityAttributes,
                [MarshalAs(UnmanagedType.U4)] FileMode dwCreationDisposition,
                [MarshalAs(UnmanagedType.U4)] FileAttributes dwFlagsAndAttributes,
                IntPtr hTemplateFile);

        public static IntPtr LoadFile(string path)
        {
            var length = (int)new FileInfo(path).Length;
            var bufferPtr = Marshal.AllocHGlobal(length);
            var nativeOverlapped = new NativeOverlapped { OffsetLow = 0, OffsetHigh = 0 };

            var handle = CreateFile(path, FileAccess.Read, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);

            try
            {
                if (!ReadFile(handle, bufferPtr, (uint)length, out uint bytesRead, ref nativeOverlapped))
                {
                    throw new Exception($"Unable to read volume. Error code: {Marshal.GetLastWin32Error()}");
                }

                return bufferPtr;
            }
            catch
            {
                Marshal.FreeHGlobal(bufferPtr);
                throw;
            }
            finally
            {
                handle.Close();
            }
        }

        public static DateTime UnixTimeMinValue => new DateTime(1970, 1, 1);

        public static void Destroy<T>(this IntPtr ptr)
            => Marshal.DestroyStructure(ptr, typeof(T));

        public static IntPtr ToIntPtr<T>(this T obj)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
            Marshal.StructureToPtr(obj, ptr, false);
            return ptr;
        }
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static string GetDiagInfo(this byte[] data, string field)
        {
            data = data.Take(8).ToArray();
            var hexStr = BitConverter.ToString(data).Replace("-", "");
            return $"{field,-20}{hexStr,-20}";
        }
        public static string GetDiagIPAddr(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            return $"{arr.GetDiagInfo(field)}{new IPAddress(BitConverter.ToUInt32(arr, 0))}";
        }

        public static string GetDiagInt32(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            return $"{arr.GetDiagInfo(field)}{BitConverter.ToInt32(arr, 0)}";
        }

        public static string GetDiagString(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            return $"{arr.GetDiagInfo(field)}{Encoding.Default.GetString(arr)}";
        }

        public static string GetDiagUInt64(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            return $"{arr.GetDiagInfo(field)}{BitConverter.ToUInt64(arr, 0)}";
        }

        public static string GetDiagUnixTime(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            var timestamp = UnixTimeMinValue.AddSeconds(BitConverter.ToUInt64(arr, 0));
            return $"{arr.GetDiagInfo(field)}{timestamp.ToString("dd.MM.yyyy hh:mm:ss")}";
        }

        public static string GetDiagUInt32(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            return $"{arr.GetDiagInfo(field)}{BitConverter.ToUInt32(arr, 0)}";
        }
        public static string GetDiagFloat(this IntPtr ptr, string field, int size, int offset, int length)
        {
            var arr = ptr.GetManagedArray(size, offset, length);
            return $"{arr.GetDiagInfo(field)}{BitConverter.ToSingle(arr, 0)}";
        }

        public static string GetDiagBufferAsHex(this IntPtr ptr, int size)
            => BitConverter.ToString(ptr.GetManagedArray(size, 0, size)).Replace("-", "");

        private static byte[] GetManagedArray(this IntPtr ptr, int size, int offset, int length)
        {
            byte[] arr = new byte[size];
            Marshal.Copy(ptr, arr, 0, size);

            var sa = arr.SubArray(offset, length);
            return sa;
        }
    }
}
