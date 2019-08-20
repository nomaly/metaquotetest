using MetaQuoteTest.Helpers;
using MetaQuoteTest.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                //PrintCityLocations(geobase);
                PrintOrderedCities(geobase);
                PrintOrderedIpAddr(geobase);

                TestFindSingleItemByCity(geobase);
                TestFindMultipleItemByCity(geobase);
                TestFindSingleItemByIp(geobase);
                //TestPerformanceByCity(geobase);
            }

            Console.ReadLine();
        }

        private static void TestPerformanceByCity(Geobase geobase)
        {
            Console.WriteLine("Test performance by city:");
            var requestCount = 100000;
            var taskCount = Environment.ProcessorCount;

            var task = Enumerable.Range(1, taskCount)
                .Select(p => Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is working");

                    var swt = new Stopwatch();
                    swt.Reset();
                    swt.Start();
                    for (var i = 0; i < requestCount; i++)
                    {
                        geobase.FindByCity("cit_O Ynolit Ra").ToArray();
                    }
                    swt.Stop();
                    return swt.ElapsedMilliseconds;
                }, TaskCreationOptions.LongRunning)).ToArray();

            Task.WaitAll(task);

            var midElapsed = task.Sum(x => x.Result) / taskCount;
            double totalRequest = requestCount * taskCount;

            Console.WriteLine($"\nmid elapsed - {midElapsed}");

            var bandwidthPerrequest = midElapsed / totalRequest;
            var bandwidthPerDay = 24 * 3600 * 1000 / bandwidthPerrequest;

            var format = new NumberFormatInfo { NumberGroupSeparator = " " };
            Console.WriteLine($"{bandwidthPerDay.ToString("n", format)} requests per day for this machine\n\n");
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
            Console.WriteLine($" elapsed - {sw.ElapsedMilliseconds} ms");
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

            var foundLocation = new List<GLocation>(geobase.FindByCity("cit_O Ynolit Ra"));

            Console.WriteLine(foundLocation.First().GetDebugString());
            Console.WriteLine($"\n");
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

            Console.WriteLine($"{"Count", 25}     City");
            foreach (var item in group.Take(10))
            {
                Console.WriteLine($"{item.City, 25}     {item.Count, -6}");
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
