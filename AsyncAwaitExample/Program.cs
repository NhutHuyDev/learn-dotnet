using System.Diagnostics;

namespace AsyncAwaitExample
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var t1 = Deplay1Async();
            var t2 = Deplay2Async();
            var t3 = Deplay3Async();

            await t1;
            await t2;
            await t3;

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed}");
        }

        private static async Task Deplay1Async()
        {
            Console.WriteLine("Delay 1...");
            await Task.Delay(1000);
            Console.WriteLine("Delay 1 Done...");
        }

        private static async Task Deplay2Async()
        {
            Console.WriteLine("Delay 2...");
            await Task.Delay(2000);
            Console.WriteLine("Delay 2 Done...");
        }

        private static async Task Deplay3Async()
        {
            Console.WriteLine("Delay 2...");
            await Task.Delay(3000);
            Console.WriteLine("Delay 2 Done...");
        }
    }
}
