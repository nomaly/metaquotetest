using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest.Model.Comparers
{
    public interface IGComparer<TKey, TCompare>
    {
        int Compare(TKey key, TCompare target);
    }
}
