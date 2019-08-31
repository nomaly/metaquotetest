using System.Runtime.InteropServices;
using System.Text;
using GeobaseModel.Helpers;

namespace GeobaseModel
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.Location.Size)]
    public struct GLocation
    {
        [FieldOffset(GeobaseOffsets.Location.Country)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 8)]
        byte[] _country;
        [FieldOffset(GeobaseOffsets.Location.Region)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 12)]
        byte[] _region;
        [FieldOffset(GeobaseOffsets.Location.Postal)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 12)]
        byte[] _postal;
        [FieldOffset(GeobaseOffsets.Location.City)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 24)]
        byte[] _city;
        [FieldOffset(GeobaseOffsets.Location.Organization)]
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 32)]
        byte[] _organization;
        [FieldOffset(GeobaseOffsets.Location.Latitude)]
        float _latitude;
        [FieldOffset(GeobaseOffsets.Location.Longitude)]
        float _longitude;

        public string Country
            => Encoding.Default.GetString(_country).TrimEnd('\0');

        public string Region
            => Encoding.Default.GetString(_region).TrimEnd('\0');

        public string Postal
            => Encoding.Default.GetString(_postal).TrimEnd('\0');

        public string City
            => Encoding.Default.GetString(_city).TrimEnd('\0');


        public string Organization => Encoding.Default.GetString(_organization).TrimEnd('\0');

        public float Latitude => _latitude;
        public float Longitude => _longitude;

        public string GetDebugString()
        {
            var ptr = this.ToIntPtr();
            var sb = new StringBuilder();
            sb.AppendLine(ptr.GetDiagString("Country", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.Country, 8));
            sb.AppendLine(ptr.GetDiagString("Region", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.Region, 12));
            sb.AppendLine(ptr.GetDiagString("Postal", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.Postal, 12));
            sb.AppendLine(ptr.GetDiagString("City", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.City, 24));
            sb.AppendLine(ptr.GetDiagString("Organization", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.Organization, 32));
            sb.AppendLine(ptr.GetDiagFloat("Latitude", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.Latitude, 4));
            sb.AppendLine(ptr.GetDiagFloat("Longitude", GeobaseOffsets.Location.Size, GeobaseOffsets.Location.Longitude, 4));
            ptr.Destroy<GLocation>();
            return sb.ToString();
        }
    }

    public static partial class GeobaseOffsets
    {
        public static class Location
        {
            public const int Size = 96;
            public const int Country = 0;
            public const int Region = Country + 8;
            public const int Postal = Region + 12;
            public const int City = Postal + 12;
            public const int Organization = City + 24;
            public const int Latitude = Organization + 32;
            public const int Longitude = Latitude + 4;
        }
    }
}
