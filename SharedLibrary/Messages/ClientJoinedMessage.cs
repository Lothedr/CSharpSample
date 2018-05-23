using System;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class ClientJoinedMessage : Message
    {
        public string ClientName { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            Console.WriteLine($"Unsupported message {nameof(ClientJoinedMessage)}");
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.Print($"{ClientName} joined.");
        }
    }
}