using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class JoinRoomMessage : Message
    {
        public string RoomKey { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            lock (serverKnowledge)
            {
                if (!serverKnowledge.ClientToName.ContainsKey(Sender))
                {
                    Sender.SendMessageAsync(new ErrorMessage("You must register first!"));
                    return;
                }
                var previousRoom = serverKnowledge.Rooms.FirstOrDefault(p => p.Value.Contains(Sender));
                if (!serverKnowledge.Rooms.TryGetValue(RoomKey, out HashSet<IMessageStream> room))
                {
                    room = new HashSet<IMessageStream>();
                    serverKnowledge.Rooms.Add(RoomKey, room);
                }
                if (room.Contains(Sender))
                {
                    Sender.SendMessageAsync(new ErrorMessage($"You are already in the room \"{RoomKey}\"."));
                }
                else
                {
                    string senderName = serverKnowledge.ClientToName[Sender];
                    if (!previousRoom.Equals(new KeyValuePair<string, HashSet<IMessageStream>>()))
                    {
                        foreach (var messageStream in previousRoom.Value.Where(c => c != Sender))
                        {
                            messageStream.SendMessageAsync(new ClientExitedMessage { ClientName = senderName });
                        }
                        previousRoom.Value.Remove(Sender);
                        Console.WriteLine($"{senderName} left {previousRoom.Key}.");
                    }
                    Sender.SendMessageAsync(new RoomJoinConfirnMessage { RoomKey = RoomKey });
                    foreach (var messageStream in room)
                    {
                        messageStream.SendMessageAsync(new ClientJoinedMessage { ClientName = senderName });
                    }
                    room.Add(Sender);
                    Console.WriteLine($"{senderName} joined {RoomKey}.");
                }
            }
        }

        public override void UpdateClient(ConsolePlus console)
        {
            throw new NotImplementedException();
        }
    }
}