using MetaQuoteTest.Algo;
using MetaQuoteTest.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace MetaQuoteTest.Model
{
    public sealed class Geobase : IDisposable
    {
        private UnmanagedBuffer _buffer;
        private BinarySearchIterativeAlgorithm _finder;

        public IntPtr Ptr => _buffer.Ptr;
               
        private Geobase(UnmanagedBuffer buffer)
        {
            _buffer = buffer;
            _finder = new BinarySearchIterativeAlgorithm();
        }

        public GHeader Header
            => Marshal.PtrToStructure<GHeader>(_buffer.Ptr);
        
        public IEnumerable<GLocation> Locations
        {
            get
            {
                var totalRecords = Header.Records;
                for (var i = 0; i < totalRecords; i++)
                {
                    yield return GetLocation(i);
                }

                yield break;
            }
        }

        public IEnumerable<GIpInterval> Intervals
        {
            get
            {
                var totalRecords = Header.Records;
                for (var i = 0; i < totalRecords; i++)
                {
                    yield return GetInterval(i);
                }

                yield break;
            }
        }
        
        public IEnumerable<GCityLocation> CityLocations
        {
            get
            {
                var totalRecords = Header.Records;
                for (var i = 0; i < totalRecords; i++)
                {
                    yield return GetCityLocation(i);
                }

                yield break;
            }
        }
        public GCityLocation GetCityLocation(int idx)
        {
            var tgtIntPtr = GetPointer((int)Header.OffsetCities + GeobaseOffsets.CityLocation.Size * idx);
            return Marshal.PtrToStructure<GCityLocation>(tgtIntPtr);
        }

        public GIpInterval GetInterval(int idx)
        {
            var tgtIntPtr = GetPointer((int)Header.OffsetRanges + GeobaseOffsets.IpInterval.Size * idx);
            return Marshal.PtrToStructure<GIpInterval>(tgtIntPtr);
        }

        public GLocation GetLocation(int idx)
        {
            var tgtIntPtr = GetPointer((int)Header.OffsetLocation + GeobaseOffsets.Location.Size * idx);
            return Marshal.PtrToStructure<GLocation>(tgtIntPtr);
        }

        public IntPtr GetPointer(int offset)
            => _buffer.Ptr + offset;

        public static Geobase Load(Stream stream)
            => new Geobase(new UnmanagedBuffer(stream));

        public void Dispose()
        {
            _buffer.Dispose();
        }
    }
}
