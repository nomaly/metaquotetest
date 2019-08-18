using MetaQuoteTest.Algo;
using MetaQuoteTest.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace MetaQuoteTest.Model
{
    unsafe public sealed class Geobase : IDisposable
    {
        private UnmanagedBuffer _buffer;
        private BinarySearchIterativeAlgorithm _finder;

        public IntPtr Ptr => _buffer.Ptr;
               
        private Geobase(UnmanagedBuffer buffer)
        {
            _buffer = buffer;
            _finder = new BinarySearchIterativeAlgorithm();
        }

        public GHeader GetHeader()
            => Marshal.PtrToStructure<GHeader>(_buffer.Ptr);

        public IEnumerable<GLocation> GetLocations()
        {
            var totalRecords = GetHeader().Records;
            for (var i = 0; i < totalRecords; i++)
            {
                yield return GetLocation(i);
            }

            yield break;
        }

        public GLocation GetLocation(int idx)
        {
            var header = GetHeader();
            var tgtIntPtr = GetPointer((int)header.OffsetLocation + GeobaseOffsets.Location.Size * idx);
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
