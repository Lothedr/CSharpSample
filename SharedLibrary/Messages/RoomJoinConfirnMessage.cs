using System;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class RoomJoinConfirnMessage : Message
    {
        public string RoomKey { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            Console.WriteLine($"Unsupported message {nameof(RoomJoinConfirnMessage)}.");
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.RoomKey = RoomKey;
            console.Print($"Joined to {RoomKey}.");
        }
    }
}