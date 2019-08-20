using MetaQuoteTest.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MetaQuoteTest.Model
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.Header.Size)]
    public struct GHeader
    {
        [FieldOffset(GeobaseOffsets.Header.Version)]
        int _version;
        [FieldOffset(GeobaseOffsets.Header.Name)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 32)]
        byte[] _name;
        [FieldOffset(GeobaseOffsets.Header.Timestamp)]
        ulong _timestamp;
        [FieldOffset(GeobaseOffsets.Header.Records)]
        int _records;
        [FieldOffset(GeobaseOffsets.Header.OffsetRanges)]
        uint _offsetRanges;
        [FieldOffset(GeobaseOffsets.Header.OffsetCities)]
        uint _offsetCities;
        [FieldOffset(GeobaseOffsets.Header.OffsetLocation)]
        uint _offsetLocation;

        public int Verision => _version;
        public string Name => Encoding.Default.GetString(_name);
        public ulong Timestamp => _timestamp;
        public int Records => _records;
        public uint OffsetRanges => _offsetRanges;
        public uint OffsetCities => _offsetCities;
        public uint OffsetLocation => _offsetLocation;

        public string GetDebugString()
        {
            var ptr = this.ToIntPtr();
            var sb = new StringBuilder();
            sb.AppendLine(ptr.GetDiagInt32("Version", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.Version, 4));
            sb.AppendLine(ptr.GetDiagString("Name", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.Name, 32));
            sb.AppendLine(ptr.GetDiagUInt64("Timestamp", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.Timestamp, 8));
            sb.AppendLine(ptr.GetDiagInt32("Records", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.Records, 4));
            sb.AppendLine(ptr.GetDiagUInt32("OffsetRanges", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.OffsetRanges, 4));
            sb.AppendLine(ptr.GetDiagUInt32("OffsetCities", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.OffsetCities, 4));
            sb.AppendLine(ptr.GetDiagUInt32("OffsetLocation", GeobaseOffsets.Header.Size, GeobaseOffsets.Header.OffsetLocation, 4));
            sb.AppendLine();
            ptr.Destroy<GHeader>();
            return sb.ToString();
        }
    }

    public static partial class GeobaseOffsets
    {
        public static class Header
        {
            public const int Size = 60;
            public const int Version = 0;
            public const int Name = Version + 4;
            public const int Timestamp = Name + 32;
            public const int Records = Timestamp + 8;
            public const int OffsetRanges = Records + 4;
            public const int OffsetCities = OffsetRanges + 4;
            public const int OffsetLocation = OffsetCities + 4;
        }
    }
}
