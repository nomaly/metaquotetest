using MetaQuoteTest.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MetaQuoteTest
{
    class Program
    {
        public const string Path = "C:\\Users\\i.spiridonov\\Downloads\\geobase.dat";
        unsafe static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var geobase = Geobase.Load(fs))
            {
                sw.Stop();
                Console.WriteLine($"Load time: {sw.ElapsedMilliseconds} ms\n\n");
                PrintInfo(geobase);
                sw.Reset();

                var location = geobase.GetLocation(10);
                Console.WriteLine(location.GetDebugString());

                foreach (var lIdx in geobase.CityLocation.Take(5))
                {
                    Console.WriteLine(lIdx.GetDebugString());
                }

                //var foundLocation = new List<GLocation>(geobase.FindByCity("cou_UXU"));
            }

            Console.ReadLine();
        }

        private static void PrintInfo(Geobase geobase)
        {
            Console.WriteLine(geobase.Header.GetDebugString());
        }
    }
}
