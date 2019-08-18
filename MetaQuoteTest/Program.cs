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
        public const string Path = "C:\\Users\\Nom\\Desktop\\geobase.dat";
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
            }

            Console.ReadLine();
        }
        
        private static void PrintInfo(Geobase geobase)
        {
            Console.WriteLine("Header");
            Console.WriteLine($"Version         {geobase.GetHeader().Verision}");
            Console.WriteLine($"Name            {geobase.GetHeader().Name}");
            Console.WriteLine($"Timestamp       {geobase.GetHeader().Timestamp}");
            Console.WriteLine($"Records         {geobase.GetHeader().Records}");
            Console.WriteLine($"OffsetRanges    {geobase.GetHeader().OffsetRanges}");
            Console.WriteLine($"OffsetCities    {geobase.GetHeader().OffsetCities}");
            Console.WriteLine($"OffsetLocation  {geobase.GetHeader().OffsetLocation}");
            Console.WriteLine();

            Console.WriteLine("Locations");
            var location = geobase.GetLocation(0);

            Console.WriteLine("Locations debug");
            Console.WriteLine(location.GetDebugString(geobase.GetPointer((int)geobase.GetHeader().OffsetLocation)));
        }
    }
}
