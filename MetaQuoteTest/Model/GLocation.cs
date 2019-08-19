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
        [FieldOffset(GeobaseOffsets.Location.Region)]
        fixed sbyte _region[12];
        [FieldOffset(GeobaseOffsets.Location.Postal)]
        fixed sbyte _postal[12];
        [FieldOffset(GeobaseOffsets.Location.City)]
        fixed sbyte _city[24];
        [FieldOffset(GeobaseOffsets.Location.Organization)]
        fixed sbyte _organization[32];
        [FieldOffset(GeobaseOffsets.Location.Latitude)]
        float _latitude;
        [FieldOffset(GeobaseOffsets.Location.Longitude)]
        float _longitude;

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
        public string Region
        {
            get
            {
                fixed (sbyte* namePtr = _region)
                {
                    return Marshal.PtrToStringAnsi((IntPtr)namePtr, 12);
                }
            }
        }
        public string Postal
        {
            get
            {
                fixed (sbyte* namePtr = _postal)
                {
                    return Marshal.PtrToStringAnsi((IntPtr)namePtr, 12);
                }
            }
        }
        public string City
        {
            get
            {
                fixed (sbyte* namePtr = _city)
                {
                    return Marshal.PtrToStringAnsi((IntPtr)namePtr, 24);
                }
            }
        }


        public string Organization
        {
            get
            {
                fixed (sbyte* namePtr = _organization)
                {
                    return Marshal.PtrToStringAnsi((IntPtr)namePtr, 32);
                }
            }
        }

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
