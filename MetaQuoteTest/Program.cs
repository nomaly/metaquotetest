using MetaQuoteTest.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MetaQuoteTest
{
    class Program
    {
        public const string Path = "D:\\Downloads\\geobase.dat";
        unsafe static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var geobase = Geobase.Load(fs))
            {
                sw.Stop();
                Console.WriteLine($"Load time: {sw.ElapsedMilliseconds} ms");
                PrintInfo(geobase);

                sw.Reset();
                sw.Start();
                var cities = geobase.Locations.Select(x => x.City).ToArray();
                
                sw.Stop();
                Console.WriteLine($"Sort time: {sw.ElapsedMilliseconds} ms");
            }

            Console.ReadLine();
        }

        private static void PrintInfo(Geobase geobase)
        {
            Console.WriteLine("Header");
            Console.WriteLine($"Version         {geobase.Header.Verision}");
            Console.WriteLine($"Name            {geobase.Header.Name}");
            Console.WriteLine($"Timestamp       {geobase.Header.Timestamp}");
            Console.WriteLine($"Records         {geobase.Header.Records}");
            Console.WriteLine($"OffsetRanges    {geobase.Header.OffsetRanges}");
            Console.WriteLine($"OffsetCities    {geobase.Header.OffsetCities}");
            Console.WriteLine($"OffsetLocation  {geobase.Header.OffsetLocation}");
            Console.WriteLine();

            Console.WriteLine("Locations");
            var loc = geobase.GetLocation(1);
            Console.WriteLine($"Country               {loc.Country}");
            Console.WriteLine($"Region                {loc.Region}");
            Console.WriteLine($"Postal                {loc.Postal}");
            Console.WriteLine($"City                  {loc.City}");
            Console.WriteLine($"Organization          {loc.Organization}");
            Console.WriteLine($"Longitude             {loc.Longitude}");
            Console.WriteLine($"Latitude              {loc.Latitude}");
            Console.WriteLine();

            var ivl = geobase.GetInterval(5);
            Console.WriteLine("Intervals");
            Console.WriteLine($"Ipfrom              {ivl.IpAddrFrom}");
            Console.WriteLine($"IpTo                {ivl.IpAddrTo}");
            Console.WriteLine($"LocationIndex       {ivl.LocationIndex}");
            Console.WriteLine();
        }

        static void Sort(ref int[] arr)
        {
            int i, j;
            int[] tmp = new int[arr.Length];
            for (int shift = 31; shift > -1; --shift)
            {
                j = 0;
                for (i = 0; i < arr.Length; ++i)
                {
                    bool move = (arr[i] << shift) >= 0;
                    if (shift == 0 ? !move : move)
                        arr[i - j] = arr[i];
                    else
                        tmp[j++] = arr[i];
                }
                Array.Copy(tmp, 0, arr, arr.Length - j, j);
            }
        }
    }
}
