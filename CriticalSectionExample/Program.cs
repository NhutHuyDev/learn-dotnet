namespace CriticalSectionExample
{
    public class Program
    {
        private static int x = 10;
        private static int y = 20;

        private static readonly object lockObject = new object();

        enum Options {
            WithLock,
            WithMonitor
        }

        static void Main(string[] args)
        {
            var t = new Thread(new ThreadStart(P)) { IsBackground = true };
            t.Start();

            PrintXY(Options.WithMonitor);
            Swap(Options.WithMonitor);
            PrintXY(Options.WithMonitor);

            Console.ReadLine();
        }

        private static void Swap(Options option = Options.WithLock)
        {
            switch (option)
            {
                case Options.WithLock:
                    
                    lock (lockObject)
                    {
                        // Critical Section 
                        int t = x;
                        Thread.Sleep(1000);
                        x = y;
                        Thread.Sleep(2000);
                        y = t;
                        // Critical Section 
                    }

                    return;

                case Options.WithMonitor:

                    bool lockTaken = false;
                    try
                    {
                        Monitor.Enter(lockObject, ref lockTaken);
                        // Critical Section
                        int t = x;
                        Thread.Sleep(1000);
                        x = y;
                        Thread.Sleep(2000);
                        y = t;
                        // Critical Section
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(lockObject);
                        }
                    }

                    return;
            }
        }

        private static void PrintXY(Options option = Options.WithLock)
        {
            switch (option)
            {
                case Options.WithLock:
                    lock (lockObject)
                    {
                        // Critical Section 
                        Console.WriteLine($"x = {x}, y = {y}");
                        // Critical Section     
                    }
                    return;

                case Options.WithMonitor:

                    bool lockTaken = false;
                    try
                    {
                        Monitor.Enter(lockObject, ref lockTaken);
                        // Critical Section
                        Console.WriteLine($"x = {x}, y = {y}");
                        // Critical Section
                    }
                    finally
                    {
                        if (lockTaken)
                        {
                            Monitor.Exit(lockObject);
                        }
                    }

                    return;
            }
        }

        private static void P()
        {
            while (true)
            { 
                PrintXY(Options.WithMonitor);  
                Thread.Sleep(100);  
            }
        }
    }
}
