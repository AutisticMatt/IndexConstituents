using AutisticMatt.IndexConstituents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutisticMatt.DevConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"c:\temp\SHARADAR-SP500.csv";
            //GetCode();
            //SearchForEnums(filePath);

            var lines = File.ReadAllLines(filePath);
            var scs = SpyConstituent.FromCSV(lines);
            
            //Note that there aren't actually 500 constituents in the S&P500.  There is usually more.
            //This is because of stocks like Google.  It has 2 tickers:  GOOG and GOOGL, both are in the index.
            var spy500 = scs.Where(rd => rd.Action == SpyConstituent.ActionEnum.current).ToList();


            Console.WriteLine("Press Enter key to close, dummy.");
            Console.ReadLine();
        }

        private static void SearchForEnums(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var scs = SpyConstituent.FromCSV(lines);
            var actions = scs.Select(rd => rd.Action).Distinct();
            Trace.WriteLine($"There are {actions.Count()} distinct actions.");
            var cnt = 1;
            foreach(var ac in actions)
            {
                Trace.WriteLine($"{ac} = {cnt},");
                cnt++;
            }

        }
        private static void GetCode()
        {
            //Auto code generator.  Use it to quickly add CSV methods to a class.
            //Note that in this particular case the FromCSV field indexes are wrong.  
            //  I should have used CSVCarver instead.  It was super easy to fix in the 
            //  class so I moved on.  
            //  Also, not using the ToCSV() methods in this class.
            CodeCreator cc = new CodeCreator(typeof(SpyConstituent));
            var rslt = cc.GetAll();
            Trace.WriteLine(rslt);
        }
    }
}
