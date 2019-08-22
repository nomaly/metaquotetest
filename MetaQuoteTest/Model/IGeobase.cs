using System.Collections.Generic;

namespace MetaQuoteTest.Model
{
    public interface IGeobase
    {
        GHeader Header { get; }
        IEnumerable<GLocation> Locations { get; }
    }
}
