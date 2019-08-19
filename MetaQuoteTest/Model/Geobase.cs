using MetaQuoteTest.Helpers;
using MetaQuoteTest.Model.Comparers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace MetaQuoteTest.Model
{
    public sealed class Geobase : IDisposable
    {
        private UnmanagedBuffer _buffer;
        private readonly GeobaseIndex<GCityLocation, string, GLocation> CityIndex;
        private readonly GeobaseIndex<GIpInterval, string, GLocation> IpIntervalIndex;

        public IntPtr Ptr => _buffer.Ptr;

        public GHeader Header
            => Marshal.PtrToStructure<GHeader>(_buffer.Ptr);

        private IntPtr Locations
            => GetPointer((int)Header.OffsetLocation);

        private IntPtr Cities
            => GetPointer((int)Header.OffsetCities);

        private IntPtr IpIntervals
            => GetPointer((int)Header.OffsetRanges);

        private Geobase(UnmanagedBuffer buffer)
        {
            _buffer = buffer;

            CityIndex = new GeobaseIndex<GCityLocation, string, GLocation>(Cities, Header.Records, new GCityComparer());
            IpIntervalIndex = new GeobaseIndex<GIpInterval, string, GLocation>(IpIntervals, Header.Records, new GIpComparer());
        }

        public IEnumerable<GLocation> FindByIp(string address)
        {
            var recIntPtr = GetPointer(Header.OffsetCities);
            var idxList = IpIntervalIndex.Find(recIntPtr, Header.Records, address);
            foreach (var idx in idxList)
            {
                yield return GetLocation(idx);
            }
        }

        public IEnumerable<GLocation> FindByCity(string city)
        {
            var recIntPtr = GetPointer(Header.OffsetCities);
            var idxList = CityIndex.Find(recIntPtr, Header.Records, city);
            foreach (var idx in idxList)
            {
                yield return GetLocation(idx);
            }
        }

        private GLocation GetLocation(int idx)
        {
            var tgtIntPtr = GetPointer(Header.OffsetLocation + GeobaseOffsets.Location.Size * idx);
            return Marshal.PtrToStructure<GLocation>(tgtIntPtr);
        }

        private IntPtr GetPointer(long offset)
            => GetPointer((int)offset);

        private IntPtr GetPointer(uint offset)
            => GetPointer((int)offset);

        private IntPtr GetPointer(int offset)
            => _buffer.Ptr + offset;
        
        public static Geobase Load(Stream stream)
            => new Geobase(new UnmanagedBuffer(stream));

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
