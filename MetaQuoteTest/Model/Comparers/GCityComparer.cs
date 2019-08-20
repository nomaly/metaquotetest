using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest.Model.Comparers
{
    public class GCityComparer : IGComparer<string, GLocation>
    {
        public int Compare(string key, GLocation target)
            => string.Compare(key, target.City);
    }
}
