using System;
using System.Collections.Generic;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class ChatRoomMessage : Message
    {
        public string RoomKey { get; set; }
        public string SenderName { get; set; }
        public string Value { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            lock (serverKnowledge)
            {
                if (!serverKnowledge.ClientToName.ContainsKey(Sender))
                {
                    Sender.SendMessageAsync(new ErrorMessage("You must register first!"));
                    return;
                }
                if (serverKnowledge.Rooms.TryGetValue(RoomKey, out HashSet<IMessageStream> room))
                {
                    if (room.Contains(Sender))
                    {
                        SenderName = serverKnowledge.ClientToName[Sender];
                        Console.WriteLine($"{SenderName} send: \"{Value}\" to room \"{RoomKey}\".");
                        foreach (var messageStream in room)
                        {
                            messageStream.SendMessageAsync(this);
                        }
                    }
                    else
                    {
                        Sender.SendMessageAsync(new ErrorMessage("You must join to room first!"));
                    }
                }
                else
                {
                    Sender.SendMessageAsync(new ErrorMessage($"Room \"{RoomKey}\" does not exists."));
                }
            }
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.Print($"{SenderName}: {Value}");
        }
    }
}