using MetaQuoteTest.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MetaQuoteTest.Model
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.Location.Size)]
    unsafe public struct GLocation
    {
        [FieldOffset(GeobaseOffsets.Location.Country)]
        fixed sbyte _country[8];
        public string Country
        {
            get
            {
                fixed (sbyte* namePtr = _country)
                {
                    return Marshal.PtrToStringAnsi((IntPtr)namePtr, 8);
                }
            }
        }

        public string GetDebugString(IntPtr ptr)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ptr.GetDiagString("Country", GeobaseOffsets.Location.Country, 8));
            sb.AppendLine();
            sb.AppendLine(ptr.GetDiagBufferAsHex(GeobaseOffsets.Location.Size));
            return sb.ToString();
        }
    }

    public static partial class GeobaseOffsets
    {
        public static class Location
        {
            public const int Size = 96;
            public const int Country = 0;
        }
    }
}
