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
        static int realise;
        public static string fileRoutes = @"..\..\FileByThread.txt";

        // A semaphore that simulates a limited resource pool
        private static Semaphore _pool;

        static void Main(string[] args)
        {
            GenerateRoutes generateRoutes = new GenerateRoutes();
            Thread t1 = new Thread(() => generateRoutes.GenerateNumbers());
            t1.Start();
            Thread t2 = new Thread(() => generateRoutes.BestRotes());
            t2.Start();

            t1.Join();
            t2.Join();
            // Create a semaphore that can satisfy up to two
            // concurrent requests. Use an initial count of zero,
            // so that the entire semaphore count is initially
            // owned by the main program thread.
            _pool = new Semaphore(0, 2);

            // Create and start ten numbered threads. 
            for (int i = 1; i <= 10; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Loading));

                // Start the thread, passing the number.
                t.Start(i);
            }

            // Wait for half a second, to allow all the
            // threads to start and to block on the semaphore.
            Thread.Sleep(500);

            // The main thread starts out holding the entire
            // semaphore count. Calling Release(3) brings the 
            // semaphore count back to its maximum value, and
            // allows the waiting threads to enter the semaphore,
            // up to three at a time.
            //
            Console.WriteLine("\nMain thread calls Release(2).");
            _pool.Release(2);
            //realise = _pool.Release(2);
            //Console.WriteLine(realise);
            
            Console.ReadLine();
        }
        private static void Loading(object num)
        {
            // Each worker thread begins by requesting the
            // semaphore.
            
            Console.WriteLine("Truck {0} is ready and waits for the semaphore.", num);
            _pool.WaitOne();

            Console.WriteLine("Trunk {0} start loading", num);
            int time = random.Next(500, 5001);
            Thread.Sleep(time);

            // The thread's "work" consists of sleeping for 
            // about a second. Each thread "works" a little 
            // longer, just to make the output more orderly.
            //
            Console.WriteLine("Trunk {0} is loaded. Duration: {1} miliseconds", num, time);
            _pool.Release();
        }
    }
}
