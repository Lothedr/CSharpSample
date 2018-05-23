using System;
using System.Net;
using System.Threading;
using SharedLibrary;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8500;
            IPAddress address = IPAddress.Loopback;
            if (args.Length > 0 && !int.TryParse(args[0], out port))
            {
                Console.WriteLine($"Could not parse {args[0]} to int.");
                port = 8500;
            }
            if (args.Length > 1 && !IPAddress.TryParse(args[1], out address))
            {
                Console.WriteLine($"Could not parse {args[1]} to IPAddress.");
                address = IPAddress.Loopback;
            }
            Console.WriteLine($"Client trying connect to {address}:{port}.");
            IConnectionModule connectionModule = new TcpConnectionModule(new IPEndPoint(address, port));
            IMessageStream stream = connectionModule.Connect();
            if (stream == null)
            {
                return;
            }
            Console.WriteLine("Client connected.");
            Counting(3);
            Client client = new Client(stream);
            client.Start();
        }

        static void Counting(int sec)
        {
            Console.Write("Client starting in ");
            for (int i = sec; i > 0; --i)
            {
                Console.Write($"{i}...");
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}