using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dan_XXXVII_Bojana_Backo
{
    class Program
    {
        static Random random = new Random();
        public static string fileRoutes = @"..\..\FileByThread.txt";
        private static int release = 0;
        static EventWaitHandle waitHandle = new AutoResetEvent(false);
        static int[] times = new int[10];

        // A semaphore that simulates a limited resource pool
        private static Semaphore _pool;
        private static Semaphore _pool1;

        static void Main(string[] args)
        {
            object listLock = new object();
            GenerateRoutes generateRoutes = new GenerateRoutes();
            Thread t1 = new Thread(() => generateRoutes.GenerateNumbers());
            t1.Start();
            t1.Join();
            Thread t2 = new Thread(() => generateRoutes.BestRotes());
            t2.Start();
            t2.Join();

            Thread t3 = new Thread(() => SemaphoreTrunkLoading());
            t3.Start();
            t3.Join();

            Thread.Sleep(25000);
            
            Thread t4 = new Thread(() => SemaphoreRoute());
            t4.Start();
            if (!t3.IsAlive)
            {
                waitHandle.Set();
            }
            

            Console.ReadLine();
        }
        private static void Loading(object num)
        {
            // Each worker thread begins by requesting the
            // semaphore
            Console.WriteLine("\nTruck {0} is ready and waits for the semaphore.", num);
            _pool.WaitOne();

            Console.WriteLine("\nTrunk {0} start loading", num);
            int time = random.Next(500, 5001);
            times[(int)num - 1] = time;
            Thread.Sleep(time);
            release++;

            Console.WriteLine("\nTrunk {0} is loaded. Duration: {1} miliseconds", num, time);
            if (release > 1)
            {
                release = 0;
                _pool.Release(2);
            }
        }
        private static void RouteAssignment(object num)
        {
            // Each worker thread begins by requesting the
            // semaphore
            Console.WriteLine("\nTruck {0} got the route {1}.", num ,GenerateRoutes.bestRoutes[(int)num-1]);
            _pool1.WaitOne();

            Console.WriteLine("\nTruck {0} headed for its destination. You can expect delivery in between 500ms and 5 sec. ", num);
            int time = random.Next(500, 5001);
            Thread.Sleep(time);

            if (time <= 3000)
            {
                int unloadedTime = times[(int)num - 1];
                double b = unloadedTime / (1.5);
                Console.WriteLine("\nTrunk {0} arrived at its destination in {1} miliseconds", num, time);
                Console.WriteLine("\nTruck {0} was unloaded for {1:f2} miliseconds", num, b);
            }
            else
            {
                Console.WriteLine("\nOrder canceled! Trunk {0} returns to the beginning", num);
            }
            
            _pool1.Release(1);
        }

        private static void SemaphoreRoute()
        {
            waitHandle.WaitOne();
            _pool1 = new Semaphore(2, 2);
            for (int i = 1; i <= 10; i++)
            {
                // Create and start ten numbered threads. 
                Thread t = new Thread(new ParameterizedThreadStart(RouteAssignment));

                // Start the thread, passing the number.
                t.Start(i);
            }
        }

        private static void SemaphoreTrunkLoading()
        {
            // Create a semaphore that can satisfy up to two
            // concurrent requests. 
            _pool = new Semaphore(2, 2);

            // Create and start ten numbered threads. 
            for (int i = 1; i <= 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Loading));

                // Start the thread, passing the number.
                t.Start(i);
            }
            
        }
    }
}
