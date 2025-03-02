namespace EventWaitHandleExample
{
    public class Program
    {
        static BlockingQueue<string> queue = new();    

        static void Main(string[] args)
        {
            var t1 = new Thread(EnQueueThread) { IsBackground = true };
            var t2 = new Thread(EnQueueThread) { IsBackground = true };

            t1.Start();
            t2.Start();

            string? s = null;
            for (int i = 0; i < 100000; i++)
            {
                s = i.ToString();

                queue.Enqueue(s);   
            }

            
        }

        static void EnQueueThread ()
        {
            while (true)
            {
                var s = queue.Dequeue();
                Console.WriteLine($"Q = {s}");
            }
        }
    }
}
