namespace SemaphoreExample
{
    public class Program
    {
        private static readonly Random r = new Random();
        private static int ItemsInBox = 0;
        private const int MAX = 8;

        private static readonly Semaphore semaphore = new(MAX, MAX);
        private static readonly AutoResetEvent fullEvent = new(false);

        static void Main(string[] args)
        {
            for (int i = 1; i <= 20; i++)
            {
                var t = new Thread(new ParameterizedThreadStart(MoveItemThread))
                {
                    IsBackground = true,
                };
                t.Start(i.ToString());
            }

            var t_rp = new Thread(ReplaceBox)
            {
                IsBackground = true,
            };
            t_rp.Start();

            Console.ReadLine();
        }

        private static void MoveItemThread(object? o)
        {
            var armNumber = o?.ToString() ?? "-";

            while (true)
            {
                semaphore.WaitOne();

                Console.WriteLine($"{armNumber} - Moving item...");

                Thread.Sleep(r.Next(1000, 4000));

                MoveItem();

                Console.WriteLine($"{armNumber} - Done");

                if (ItemsInBox == MAX)
                {
                    fullEvent.Set();
                }
            }
        }

        private static void MoveItem()
        {
            ItemsInBox++;
            Console.WriteLine($"Current quantity: {ItemsInBox}");
        }

        private static void ReplaceBox()
        {
            while (true)
            {
                fullEvent.WaitOne();

                Console.WriteLine("Replace with a new box");

                ItemsInBox = 0;

                semaphore.Release(MAX);
            }
        }
    }
}
