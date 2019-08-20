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
        private readonly GeobaseIndex<string, GLocation> CityIndex;
        private readonly GeobaseIndex<string, GIpInterval> IpIntervalIndex;

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

            var cityIndexData = GeobaseIndexData<GLocation>.Create(Header.Records, i => (int)GetCityLocation(i).LocationIdx, i => GetLocation(i));
            CityIndex = new GeobaseIndex<string, GLocation>(cityIndexData, new GCityComparer());

            var ipIntervalIndexData = GeobaseIndexData<GIpInterval>.Create(Header.Records, idx => idx, idx => GetIpInterval(idx));
            IpIntervalIndex = new GeobaseIndex<string, GIpInterval>(ipIntervalIndexData, new GIpComparer());
        }

        public IEnumerable<GLocation> FindByIp(string address)
        {
            var idxList = IpIntervalIndex.Find(address);
            foreach (var idx in idxList)
            {
                yield return GetLocation(idx);
            }
        }

        public IEnumerable<GLocation> FindByCity(string city)
        {
            var idxList = CityIndex.Find(city);
            foreach (var idx in idxList)
            {
                yield return GetLocation(idx);
            }
        }
        private GCityLocation GetCityLocation(int idx)
        {
            var tgtIntPtr = GetPointer(Header.OffsetCities + GeobaseOffsets.CityLocation.Size * idx);
            return Marshal.PtrToStructure<GCityLocation>(tgtIntPtr);
        }

        private GLocation GetLocation(int idx)
        {
            var tgtIntPtr = GetPointer(Header.OffsetLocation + GeobaseOffsets.Location.Size * idx);
            return Marshal.PtrToStructure<GLocation>(tgtIntPtr);
        }
        private GIpInterval GetIpInterval(int idx)
        {
            var tgtIntPtr = GetPointer(Header.OffsetRanges + GeobaseOffsets.IpInterval.Size * idx);
            return Marshal.PtrToStructure<GIpInterval>(tgtIntPtr);
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
