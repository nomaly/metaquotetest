using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest.Model.Comparers
{
    public interface IGComparer<TKey, TIndex, TCompare>
    {
        int Compare(TKey key, TIndex idxItem, TCompare target);
    }
}
