using MetaQuoteTest.Model;
using System;
using System.Diagnostics;
using System.IO;

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


            }

            Console.ReadLine();
        }

        private static void PrintInfo(Geobase geobase)
        {
            Console.WriteLine(geobase.Header.GetDebugString());
        }
    }
}
