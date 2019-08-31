using System.Collections.Generic;

namespace GeobaseModel
{
    public interface IGeobase
    {
        GHeader Header { get; }
        IEnumerable<GLocation> Locations { get; }
        IEnumerable<GLocation> FindByIp(string address);
        IEnumerable<GLocation> FindByCity(string city);
    }
}
