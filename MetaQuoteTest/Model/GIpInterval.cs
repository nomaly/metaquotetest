using System.Runtime.InteropServices;

namespace MetaQuoteTest.Model
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.IpInterval.Size)]
    unsafe public sealed class GIpInterval
    {
        [FieldOffset(GeobaseOffsets.IpInterval.IpFrom)]
        public uint IpFrom;
        [FieldOffset(GeobaseOffsets.IpInterval.IpTo)]
        public uint IpTo;
        [FieldOffset(GeobaseOffsets.IpInterval.LocationIndex)]
        public uint LocationIndex;
    }

    public static partial class GeobaseOffsets
    {
        public static class IpInterval
        {
            public const int Size = 12;
            public const int IpFrom = 0;
            public const int IpTo = IpFrom + 4;
            public const int LocationIndex = IpTo + 4;
        }
    }
}
