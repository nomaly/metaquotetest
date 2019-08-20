using MetaQuoteTest.Helpers;
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

            using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
            using (var geobase = Geobase.Load(fs))
            {
                sw.Stop();
                Console.WriteLine($"Load time: {sw.ElapsedMilliseconds} ms\n\n");
                PrintHeader(geobase);
                sw.Reset();

                PrintLocation(52537, geobase);
                PrintCityLocations(geobase);
                PrintOrderedCities(geobase);
                PrintOrderedIpAddr(geobase);

                TestFindSingleItemByCity(geobase);
                TestFindMultipleItemByCity(geobase);
                TestFindSingleItemByIp(geobase);
            }

            Console.ReadLine();
        }

        private static void PrintOrderedIpAddr(Geobase gb)
        {
            //Console.WriteLine();
            //Console.WriteLine("Ordered ip addresses:\n");
            //var group = from i in gb.IPAddresses
            //            let loc = gb.GetLocation(i.LocationIdx)
            //            group i by new
            //            {
            //                IpFrom = i.IpAddrFrom.ToString(),
            //                IpTo = i.IpAddrTo.ToString()
            //            }
            //            select new
            //            {
            //                Address = g.,
            //                Count = g.Count(),
            //                City = g.Key
            //            };

            //Console.WriteLine($"{"Count",6}     City");
            //foreach (var item in group.Take(10))
            //{
            //    Console.WriteLine($"{item.Count,6}     {item.City,-32}");
            //}
        }

        private static void TestFindSingleItemByIp(Geobase geobase)
        {
            Console.WriteLine("Test find single item by city:");
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            var foundLocation = new List<GLocation>(geobase.FindByCity("cit_O Ynolit Ra"));
            sw.Stop();

            Console.WriteLine(foundLocation.First().GetDebugString());
            Console.WriteLine($"\n elapsed - {sw.ElapsedMilliseconds} ms");
        }

        private static unsafe void TestFindMultipleItemByCity(Geobase geobase)
        {
            Console.WriteLine("Test find multiple item by city:");
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            var foundLocation = new List<GLocation>(geobase.FindByCity("cit_O Ynolit Ra"));
            sw.Stop();

            foreach (var item in foundLocation)
            {
                Console.WriteLine($"{item.Organization,30}     {item.City,-32}");
            }
            Console.WriteLine($"\n elapsed - {sw.ElapsedMilliseconds} ms\n\n");
        }

        private static unsafe void TestFindSingleItemByCity(Geobase geobase)
        {
            Console.WriteLine("Test find single item by city:");
            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            var foundLocation = new List<GLocation>(geobase.FindByCity("cit_O Ynolit Ra"));
            sw.Stop();

            Console.WriteLine(foundLocation.First().GetDebugString());
            Console.WriteLine($"\n elapsed - {sw.ElapsedMilliseconds} ms\n\n");
        }

        private static unsafe void PrintLocation(int locIdx, Geobase geobase)
        {
            var location = geobase.GetLocation(locIdx);
            Console.WriteLine($"Location {locIdx}:");
            Console.WriteLine(location.GetDebugString());
            Console.WriteLine();
        }

        private static unsafe void PrintCityLocations(Geobase geobase)
        {
            Console.WriteLine();
            foreach (var lIdx in geobase.CityLocation.Take(5)) // Дай пятюню!
            {
                Console.WriteLine(lIdx.GetDebugString());
            }
            Console.WriteLine();
        }

        private static void PrintOrderedCities(Geobase gb)
        {
            Console.WriteLine();
            Console.WriteLine("Ordered cities:\n");
            var group = from i in gb.CityOrderedLocations
                        group i by i.City into g
                        select new
                        {
                            Count = g.Count(),
                            City = g.Key,
                            Values = g
                        };

            Console.WriteLine($"{"Count", 6}     City");
            foreach (var item in group.Where(x => x.City.Contains("cit_O Ynolit Ra")).SelectMany(x => x.Values))
            {
                Console.WriteLine($"{item.Organization, 30}     {item.City, -32}");
            }
            Console.WriteLine("\n\n");
        }

        private static void PrintHeader(Geobase geobase)
        {
            Console.WriteLine("Header:");
            Console.WriteLine(geobase.Header.GetDebugString());
            Console.WriteLine();
        }
    }
}
