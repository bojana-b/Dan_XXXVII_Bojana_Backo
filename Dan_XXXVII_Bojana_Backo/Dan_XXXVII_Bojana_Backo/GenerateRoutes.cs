using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dan_XXXVII_Bojana_Backo
{
    public class GenerateRoutes
    {
        readonly object listLock = new object();
        static List<int> routesToDestination = new List<int>();
        static List<int> divisibleBy3 = new List<int>();
        public static int[] bestRoutes = new int[10];
        Random random = new Random();
        Stopwatch stopwatch = new Stopwatch();

        // Entry of 1000 randomly generated numbers from 1 to 5000
        // which represent the markings of possible routes to the destination
        public void GenerateNumbers()
        {
            int number;
            for (int i = 0; i < 1000; i++)
            {
                do
                {
                    number = random.Next(1, 5001);
                } while (routesToDestination.Contains(number));

                routesToDestination.Add(number);
            }
            Console.WriteLine("\nMarkings of possible routes to the destination: \n");
            foreach (var item in routesToDestination)
            {
                Console.Write(item + " ");
            }
            lock (listLock)
            {
                stopwatch.Start();
                WriteRoutesToFile();
                stopwatch.Stop();
                Monitor.Pulse(listLock);
            }
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
                lock (listLock)
                {
                    while(routesToDestination.Count != 1000)
                    {
                        Monitor.Wait(listLock);
                    }
                }
                using (StreamReader sr = File.OpenText(Program.fileRoutes))
                {
                    string line;
                    int num;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Int32.TryParse(line, out num);
                        if (stopwatch.ElapsedMilliseconds <= 3000)
                        {
                            if (num % 3 == 0)
                            {
                                divisibleBy3.Add(num);
                            }
                        }
                        else
                        {
                            divisibleBy3.Add(num);
                        }
                    }
                    divisibleBy3.Sort();
                    Console.WriteLine("\n\nBest routes are: \n");
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
