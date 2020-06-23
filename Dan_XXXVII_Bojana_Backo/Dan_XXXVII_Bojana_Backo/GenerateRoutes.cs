using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dan_XXXVII_Bojana_Backo
{
    public class GenerateRoutes
    {
        static List<int> routesToDestination = new List<int>();
        Random random = new Random();

        // Entry of 1000 randomly generated numbers from 1 to 5000
        // which represent the markings of possible routes to the destination
        public void GenerateNumbers()
        {
            for (int i = 0; i < 1000; i++)
            {
                int number = random.Next(1, 5001);
                routesToDestination.Add(number);
            }
            Console.WriteLine("\nMarkings of possible routes to the destination: \n");
            foreach (var item in routesToDestination)
            {
                Console.Write(item + " ");
            }
            WriteRoutesToFile();
        }

        public void WriteRoutesToFile()
        {
            try
            {
                File.Delete(Program.fileRoutes);
                using (StreamWriter sw = File.CreateText(Program.fileRoutes))
                {
                    foreach (var item in routesToDestination)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                    //Console.WriteLine(Thread.CurrentThread.Name + " has finished writing to the file!");
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }

        // Function to select the best routes 
        // Array bestRoutes represents the smallest numbers from the list divisibleBy3
        public void BestRotes()
        {
            try
            {
                using (StreamReader sr = File.OpenText(Program.fileRoutes))
                {
                    List<int> divisibleBy3 = new List<int>();
                    int[] bestRoutes = new int[10];
                    string line;
                    int num;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Int32.TryParse(line, out num);
                        if (num % 3 == 0)
                        {
                            divisibleBy3.Add(num);
                        }
                    }
                    divisibleBy3.Sort();
                    Console.WriteLine("\nBest routes are: ");
                    for (int i = 0; i < 10; i++)
                    {
                        bestRoutes[i] = divisibleBy3[i];
                        Console.WriteLine("Route_{0} -> {1}", i, bestRoutes[i]);
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (IOException e)
            {
                Console.WriteLine($"The file could not be opened: '{e}'");
            }
        }
    }
}
