using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MetaQuoteTest.Model
{
    unsafe public class GeobaseIndex<T, TKey, TStruct>
    {
        private readonly IntPtr StartIntPtr;
        private readonly int BlockSize;
        private readonly int Length;
        IGComparer<TKey, TStruct> Compare;

        public GeobaseIndex(IntPtr ptr, int length, IGComparer<TKey, TStruct> comparer)
        {
            BlockSize = Marshal.SizeOf<T>();
            StartIntPtr = ptr;
            Length = length;
            Compare = comparer;
        }

        public IEnumerable<int> Find(IntPtr startPtr, int numberOfElements, TKey key)
        {
            throw new NotImplementedException();
            //var minPtrAddr = (long)startPtr;
            //var maxPtrAddr = (long)(startPtr + length - blockSize);

            //while (minPtrAddr <= maxPtrAddr)
            //{
            //    var midPtrAddr = minPtrAddr + (maxPtrAddr - minPtrAddr) / blockSize / 2 * blockSize;
            //    var midStruct = Marshal.PtrToStructure<TStruct>((IntPtr)midPtrAddr);
            //    if (compare(key, midStruct) == 0)
            //    {
            //        return ;
            //    }
            //    else if (compare(key, midStruct) < 1)
            //    {
            //        maxPtrAddr = midPtrAddr - blockSize;
            //    }
            //    else
            //    {
            //        minPtrAddr = midPtrAddr + blockSize;
            //    }
            //}

            //yield return -1;
            //yield break;
        }
        static IEnumerable<int> BinarySearch(int[] array, int low, int high, int searchedValue)
        {
            if (low > high)
            {
                yield break;
            }

            int mid = (low + high) / 2;
            if (searchedValue < array[mid])
            {
                high = mid - 1;
                BinarySearch(array, low, high, searchedValue);
            }
            else if (searchedValue > array[mid])
            {
                low = mid + 1;
                BinarySearch(array, low, high, searchedValue);
            }
            else if (searchedValue == array[mid])
            {
                Console.WriteLine(array[mid]);
                BinarySearch(array, low, mid - 1, searchedValue);
                BinarySearch(array, mid + 1, high, searchedValue);
            }
        }
    }
}
