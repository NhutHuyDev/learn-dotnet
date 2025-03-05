using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatProtocol
{
    public class Client
    {
        private readonly IPEndPoint _endPoint;

        private readonly Socket _clientSocket;

        public Client() : this(null)
        { }

        public Client(IPAddress? ip, int port = ChatProtocol.Constants.DefaultChatPort)
        {
            ip ??= IPAddress.Loopback;

            this._endPoint = new IPEndPoint(ip, port);

            this._clientSocket = new Socket(this._endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task Run()
        {
            try
            {
                await this._clientSocket.ConnectAsync(_endPoint);
                var buffer = new byte[1024];
                var r = await this._clientSocket.ReceiveAsync(buffer);

                if (r == 0)
                {
                    showConnectionError();
                    return;
                }

                var welcomeText = Encoding.UTF8.GetString(buffer, 0, r);
                if (!Constants.WelcomeText.Equals(welcomeText))
                {
                    showConnectionError();
                    return;
                }

                Console.WriteLine(welcomeText);

                while (true)
                {
                    Console.Write("Enter your message: ");
                    var msg = Console.ReadLine();

                    if (string.IsNullOrEmpty(msg))
                    {
                        await closeConnectionAsync();
                        return;
                    }
                    else
                    {
                        var bytes = Encoding.UTF8.GetBytes(msg);
                        await this._clientSocket.SendAsync(bytes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong! \n {ex.Message}");
            }
        }

        private async Task closeConnectionAsync()
        {
            var bytes = Encoding.UTF8.GetBytes(Constants.CommandShutdown);
            await this._clientSocket.SendAsync(bytes);

            this._clientSocket.Close();
        }

        private void showConnectionError()
        {
            Console.WriteLine("Invalid protocol!");
        }
    }
}
