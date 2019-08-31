using System.Collections.Generic;
using GeobaseModel.Comparers;

namespace GeobaseModel
{
    public class GeobaseIndex<TKey, TStruct>
    {
        private readonly IGComparer<TKey, TStruct> Comparer;
        private readonly GeobaseIndexData<TStruct> IndexData;

        public GeobaseIndex(GeobaseIndexData<TStruct> indexData, IGComparer<TKey, TStruct> comparer)
        {
            IndexData = indexData;
            Comparer = comparer;
        }

        public IEnumerable<TStruct> Find(TKey key)
            => BinarySearchRecursive(key, 0, IndexData.Count - 1);

        public IEnumerable<TStruct> BinarySearchRecursive(TKey key, int min, int max)
        {
            if (min > max)
            {
                yield break;
            }

            int mid = (min + max) / 2;
            if (Comparer.Compare(key, IndexData[mid]) < 0)
            {
                max = mid - 1;

                foreach (var item in BinarySearchRecursive(key, min, max))
                {
                    yield return item;
                }
            }
            else if (Comparer.Compare(key, IndexData[mid]) > 0)
            {
                min = mid + 1;
                foreach (var item in BinarySearchRecursive(key, min, max))
                {
                    yield return item;
                }
            }
            else
            {
                yield return IndexData[mid];

                foreach (var item in BinarySearchRecursive(key, min, mid - 1))
                {
                    yield return item;
                }

                foreach (var item in BinarySearchRecursive(key, mid + 1, max))
                {
                    yield return item;
                }

                yield break;
            }
        }
    }
}
