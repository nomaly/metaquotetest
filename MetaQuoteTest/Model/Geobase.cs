using MetaQuoteTest.Helpers;
using MetaQuoteTest.Model.Comparers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        private IntPtr LocationsPtr
            => GetPointer((int)Header.OffsetLocation);

        private IntPtr CitiesPtr
            => GetPointer((int)Header.OffsetCities);

        private IntPtr IpIntervalsPtr
            => GetPointer((int)Header.OffsetRanges);

        public IEnumerable<GCityLocation> CityLocation
            => Enumerable.Range(0, Header.Records).Select(GetCityLocation);

        public IEnumerable<GLocation> CityOrderedLocations
        {
            get
            {
                foreach (var clIdx in CityLocation)
                {
                    yield return GetLocation(clIdx.LocationIdx);
                }
            }
        }

        public IEnumerable<GLocation> Locations
        {
            get
            {
                foreach (var clIdx in Enumerable.Range(0, Header.Records))
                {
                    yield return GetLocation(clIdx);
                }
            }
        }
        public IEnumerable<GIpInterval> IPAddresses
        {
            get
            {
                foreach (var clIdx in Enumerable.Range(0, Header.Records))
                {
                    yield return GetIpInterval(clIdx);
                }
            }
        }


        public Geobase() : this(UnmanagedBuffer.FromFile(Program.Path))
        {
        }

        private Geobase(UnmanagedBuffer buffer)
        {
            _buffer = buffer;

            var cityIndexData = GeobaseIndexData<GLocation>.Create(Header.Records, GetCityLocationIndex, GetLocation);
            CityIndex = new GeobaseIndex<string, GLocation>(cityIndexData, new GCityComparer());

            var ipIntervalIndexData = GeobaseIndexData<GIpInterval>.Create(Header.Records, idx => idx, GetIpInterval);
            IpIntervalIndex = new GeobaseIndex<string, GIpInterval>(ipIntervalIndexData, new GIpComparer());
        }

        public IEnumerable<GLocation> FindByIp(string address)
            => IpIntervalIndex.Find(address).Select(GetLocation);

        public IEnumerable<GLocation> FindByCity(string city)
            => CityIndex.Find(city);

        public GIpInterval GetInterval(int idx)
            => GetObject<GIpInterval>(idx, GeobaseOffsets.IpInterval.Size, Header.OffsetRanges);

        public GCityLocation GetCityLocation(int idx)
            => GetObject<GCityLocation>(idx, GeobaseOffsets.CityLocation.Size, Header.OffsetCities);

        public GLocation GetLocation(GIpInterval ipInterval)
            => GetLocation(ipInterval.LocationIdx);

        public GLocation GetLocation(int idx)
            => GetObject<GLocation>(idx, GeobaseOffsets.Location.Size, Header.OffsetLocation);

        private GIpInterval GetIpInterval(int idx)
            => GetObject<GIpInterval>(idx, GeobaseOffsets.IpInterval.Size, Header.OffsetRanges);

        private int GetCityLocationIndex(int idx)
            => GetCityLocation(idx).LocationIdx;

        private T GetObject<T>(int idx, int size, uint offset)
        {
            if (idx < 0 || idx >= Header.Records)
            {
                throw new ArgumentOutOfRangeException();
            }

            var tgtIntPtr = GetPointer(offset + size * idx);
            var result = Marshal.PtrToStructure<T>(tgtIntPtr);
            return result;
        }

        private IntPtr GetPointer(long offset)
            => GetPointer((int)offset);

        private IntPtr GetPointer(uint offset)
            => GetPointer((int)offset);

        private IntPtr GetPointer(int offset)
            => _buffer.Ptr + offset;

        public static Geobase Load(string path)
            => new Geobase(UnmanagedBuffer.FromFile(path));

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
