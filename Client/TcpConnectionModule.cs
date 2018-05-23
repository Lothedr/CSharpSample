using System;
using System.Net;
using System.Net.Sockets;
using SharedLibrary;

namespace Client
{
    public class TcpConnectionModule : IConnectionModule
    {
        private IPEndPoint endPoint;
        public TcpConnectionModule(IPEndPoint endPoint)
        {
            this.endPoint = endPoint;
        }
        public IMessageStream Connect()
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect(endPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unable connect to {endPoint}");
                return null;
            }
            return new BasicMessageStream(client.GetStream());
        }
    }
}