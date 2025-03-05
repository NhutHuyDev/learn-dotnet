using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatProtocol
{
    public class Server 
    {
        private int onlineClientCount;
        
        private List<int> _room;

        private readonly IPEndPoint _endPoint;

        private readonly Socket _server;

        public Server() : this(null)
        {}

        public Server(IPAddress? ip, int port = ChatProtocol.Constants.DefaultChatPort)
        {
            ip ??= IPAddress.Loopback;

            this.onlineClientCount = 0;

            this._room = [];

            this._endPoint = new IPEndPoint(ip, port);

            this._server = new Socket(this._endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }

        public async Task Run()
        {
            try
            {
                this._server.Bind(this._endPoint);

                Console.WriteLine($"Listening... (ip: {this._endPoint.Address}; port: {this._endPoint.Port})");

                this._server.Listen();

                CancellationTokenSource cancellationTokenSource = new();
                var cancellationToken = cancellationTokenSource.Token;

                var acceptTask = AcceptConnectionsAsync(cancellationToken);

                Console.WriteLine("Press ANY KEY to shutdown the server...");
                Console.ReadKey(true);

                cancellationTokenSource.Cancel();

                await acceptTask;

                return;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"Goodbye!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong! \n {ex.Message}");
            }
        }

        private async Task AcceptConnectionsAsync(CancellationToken cancellationToken)
        {
            var clientHandlers = new List<Task>();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var clientSocket = await this._server.AcceptAsync(cancellationToken);
                    var t = HandleClientRequestAsync(clientSocket, cancellationToken);
                    clientHandlers.Add(t);
                }
                catch 
                {
                    throw;
                }
            }

            await Task.WhenAll(clientHandlers);
        }

        private async Task HandleClientRequestAsync(Socket clientSocket, CancellationToken cancellationToken)
        {
            this.onlineClientCount += 1;

            Console.WriteLine($"[Client {onlineClientCount}] connected!");

            var welcomeBytes = Encoding.UTF8.GetBytes(Constants.WelcomeText);
            await clientSocket.SendAsync(welcomeBytes, cancellationToken);

            var buffer = new byte[1024];

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var r = await clientSocket.ReceiveAsync(buffer, cancellationToken: cancellationToken);
                    var msg = Encoding.UTF8.GetString(buffer, 0, r);

                    if (msg.Equals(Constants.CommandShutdown))
                    {
                        Console.WriteLine($"[Client {onlineClientCount}] disconnected!");
                        CloseConnection();
                        break;
                    }


                    Console.WriteLine($"[Client {onlineClientCount}]: {msg}");
                }
                catch 
                {
                    throw;
                }
            }
        }

        private void CloseConnection()
        {
            this.onlineClientCount -= 1;
            this._server.Close();
        }
    }
}
