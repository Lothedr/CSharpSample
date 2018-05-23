using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class Server
    {
        private readonly IStreamManager streamManager;
        private TcpListener listener;
        private CancellationTokenSource cancellation = new CancellationTokenSource();
        public Server(int port, IStreamManager streamManager)
        {
            listener = new TcpListener(IPAddress.Any, port);
            this.streamManager = streamManager;
        }
        public void Stop()
        {
            cancellation.Cancel();
        }
        public async void StartListeningAsync()
        {
            listener.Start(); 
            Console.WriteLine("Server started.");
            while (!cancellation.IsCancellationRequested)
            {
                ManageClientAsync(await listener.AcceptTcpClientAsync());
            }
            streamManager.Stop();
        }
        private async void ManageClientAsync(TcpClient client)
        {
            EndPoint from = client.Client.RemoteEndPoint;
            Console.WriteLine($"Client connected from {from}.");
            await streamManager.ServeStreamAsync(client.GetStream());
            Console.WriteLine($"Client disconnected from {from}.");
            client.Dispose();
        }
    }
}