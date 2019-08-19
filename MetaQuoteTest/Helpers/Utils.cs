﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest.Helpers
{
    public static class Utils
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        private static string GetDiagInfo(this byte[] data, string field)
        {
            data = data.Length > 8 ? data.SubArray(0, 8) : data;
            var hexStr = BitConverter.ToString(data).Replace("-", "");
            return $"{field,-20}{hexStr,-20}";
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