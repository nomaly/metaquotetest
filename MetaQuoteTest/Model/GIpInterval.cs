﻿using MetaQuoteTest.Helpers;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace MetaQuoteTest.Model
{
    [StructLayout(LayoutKind.Explicit, Size = GeobaseOffsets.IpInterval.Size)]
    unsafe public struct GIpInterval
    {
        [FieldOffset(GeobaseOffsets.IpInterval.IpFrom)]
        uint _ipFrom;
        [FieldOffset(GeobaseOffsets.IpInterval.IpTo)]
        uint _ipTo;
        [FieldOffset(GeobaseOffsets.IpInterval.LocationIndex)]
        uint _locationIndex;

        public uint IpFrom => _ipFrom;
        public uint IpTo => _ipTo;
        public uint LocationOffset => _locationIndex;
        public int LocationIdx
            => (int)LocationOffset / GeobaseOffsets.Location.Size;

        public IPAddress IpAddrFrom => new IPAddress(IpFrom);
        public IPAddress IpAddrTo => new IPAddress(IpTo);

        public string GetDebugString()
        {
            var ptr = this.ToIntPtr();
            var sb = new StringBuilder();
            sb.AppendLine(ptr.GetDiagString("Ipfrom", GeobaseOffsets.IpInterval.Size, GeobaseOffsets.IpInterval.IpFrom, 4));
            sb.AppendLine(ptr.GetDiagString("IpTo", GeobaseOffsets.IpInterval.Size, GeobaseOffsets.IpInterval.IpTo, 4));
            sb.AppendLine(ptr.GetDiagString("LocationIndex", GeobaseOffsets.IpInterval.Size, GeobaseOffsets.IpInterval.LocationIndex, 4));
            ptr.Destroy<GIpInterval>();
            return sb.ToString();
        }
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
