using MetaQuoteTest.Model.Comparers;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MetaQuoteTest.Model
{
    unsafe public class GeobaseIndex<TKey, TStruct>
    {
        private readonly IGComparer<TKey, TStruct> Comparer;
        private readonly GeobaseIndexData<TStruct> IndexData;

        public GeobaseIndex(GeobaseIndexData<TStruct> indexData, IGComparer<TKey, TStruct> comparer)
        {
            IndexData = indexData;
            Comparer = comparer;
        }

        public IEnumerable<int> Find(TKey key)
        {
            int min = 0;
            int max = IndexData.Count - 1;
            while (min <= max)
            {
                int mid = (min + max) / 2;
                if (Comparer.Compare(key, IndexData[mid]) == 0)
                {
                    yield return mid;
                }
                else if (Comparer.Compare(key, IndexData[mid]) < 0)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            yield break;
        }
    }
}
