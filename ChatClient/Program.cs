using ChatProtocol;

namespace ChatClient
{
    public class Program
    {
        static async Task Main()
        {
            var client = new Client();
            await client.Run();
        }
    }
}
