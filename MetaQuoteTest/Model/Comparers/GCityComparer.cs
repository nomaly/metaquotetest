using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest.Model.Comparers
{
    public class GCityComparer : IGComparer<string, GCityLocation, GLocation>
    {
        public int Compare(string key, GCityLocation idxItem, GLocation target)
        {
            throw new NotImplementedException();
        }
    }
}
