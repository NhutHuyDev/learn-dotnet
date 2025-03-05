using ChatProtocol;

namespace ChatServer
{
    internal class Program
    {
        static async Task Main()
        {
            var server = new Server();
            await server.Run();
        }
    }
}
