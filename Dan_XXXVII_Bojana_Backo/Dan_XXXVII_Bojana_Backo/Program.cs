using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dan_XXXVII_Bojana_Backo
{
    class Program
    {
        public static string fileRoutes = @"..\..\FileByThread.txt";
        static void Main(string[] args)
        {
            GenerateRoutes generateRoutes = new GenerateRoutes();
            generateRoutes.GenerateNumbers();

            generateRoutes.BestRotes();
            Console.ReadLine();
        }
    }
}
