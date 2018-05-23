using System;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class ConfirmMessage : Message
    {
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            Console.WriteLine($"Unsupported message {nameof(ConfirmMessage)}");
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.Print("Registered!");
        }
    }
}