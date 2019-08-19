using MetaQuoteTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest.Model
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.IpInterval.Size)]
    unsafe public struct GCityLocation
    {
        [FieldOffset(GeobaseOffsets.CityLocation.City)]
        uint _idx;

        public uint LocationIdx => _idx;

        public string GetDebugString(IntPtr ptr)
        {
            var sb = new StringBuilder();
            sb.AppendLine(ptr.GetDiagString("Ipfrom", GeobaseOffsets.IpInterval.Size, GeobaseOffsets.IpInterval.IpFrom, 4));
            return sb.ToString();
        }
    }

    public static partial class GeobaseOffsets
    {
        public static class CityLocation
        {
            public const int Size = 4;
            public const int City = 0;
        }
    }
}
