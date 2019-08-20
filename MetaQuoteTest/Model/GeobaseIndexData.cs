using System;

namespace MetaQuoteTest.Model
{
    public class GeobaseIndexData<TStruct>
    {
        public readonly int Count;

        private readonly Func<int, int> GetIndex;
        private readonly Func<int, TStruct> GetObject;

        private GeobaseIndexData(int count, Func<int, int> getIndex, Func<int, TStruct> getObject)
        {
            Count = count;
            GetIndex = getIndex;
            GetObject = getObject;
        }

        public TStruct this[int i]
        {
            get
            {
                if (i >= Count || i < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                var idx = GetIndex(i);
                var obj = GetObject(idx);

                return obj;
            }
        }

        public static GeobaseIndexData<TStruct> Create(int count, Func<int, int> getIndex, Func<int, TStruct> getObject)
            => new GeobaseIndexData<TStruct>(count, getIndex, getObject);
    }
}
