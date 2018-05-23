using System;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class ClientExitedMessage : Message
    {
        public string ClientName { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            Console.WriteLine($"Unsupported message {nameof(ClientExitedMessage)}.");
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.Print($"{ClientName} exited.");
        }
    }
}