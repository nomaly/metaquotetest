using System.Runtime.InteropServices;
using GeobaseModel.Helpers;

namespace GeobaseModel
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.IpInterval.Size)]
    public struct GCityLocation
    {
        [FieldOffset(GeobaseOffsets.CityLocation.City)]
        uint _idx;

        public uint LocationOffset => _idx;
        public int LocationIdx
            => (int)LocationOffset / GeobaseOffsets.Location.Size;

        public string GetDebugString()
        {
            var ptr = this.ToIntPtr();
            return ptr.GetDiagUInt32("LocationIdx", GeobaseOffsets.IpInterval.Size, GeobaseOffsets.IpInterval.IpFrom, 4);
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
