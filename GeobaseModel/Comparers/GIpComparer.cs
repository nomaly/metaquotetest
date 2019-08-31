using System;
using System.Net;

namespace GeobaseModel.Comparers
{
    public class GIpComparer : IGComparer<string, GIpInterval>
    {
        public int Compare(string key, GIpInterval target)
        {
            var intAddress = BitConverter.ToUInt32(IPAddress.Parse(key).GetAddressBytes(), 0);
            if (intAddress < target.IpFrom)
            {
                return -1;
            }

            if (intAddress > target.IpTo)
            {
                return 1;
            }

            return 0;
        }
    }
}
