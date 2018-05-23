using System;
using SharedLibrary;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8500;
            if (args.Length > 0 && !int.TryParse(args[0], out port))
            {
                Console.WriteLine($"Could not parse {args[0]} to int.");
                port = 8500;
            }
            Console.WriteLine($"Starting server on port {port}. Press Q to exit.");
            ServerKnowledge knowledge = new ServerKnowledge();
            IStreamManager streamManager = new BasicStreamManager(knowledge);
            Server server = new Server(port, streamManager);
            server.StartListeningAsync();
            while (Console.ReadKey(true).Key != ConsoleKey.Q)
            {
                //waitnig for Q
            }
            server.Stop();
        }
    }
}
