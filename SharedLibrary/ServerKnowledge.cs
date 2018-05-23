using System.Collections.Generic;
using System.Linq;
using SharedLibrary.Messages;

namespace SharedLibrary
{
    public class ServerKnowledge
    {
        public Dictionary<IMessageStream, string> ClientToName { get; } = new Dictionary<IMessageStream, string>();
        public Dictionary<string, HashSet<IMessageStream>> Rooms { get; } = new Dictionary<string, HashSet<IMessageStream>>();
        public void ClientDisconnected(IMessageStream messageStream)
        {
            lock (this)
            {
                if (ClientToName.Remove(messageStream, out string name))
                {
                    var previousRoom = Rooms.FirstOrDefault(p => p.Value.Contains(messageStream));
                    if (!previousRoom.Equals(new KeyValuePair<string, HashSet<IMessageStream>>()))
                    {
                        previousRoom.Value.Remove(messageStream);
                        foreach (var stream in previousRoom.Value)
                        {
                            stream.SendMessageAsync(new ClientExitedMessage { ClientName = name });
                        }
                    }
                }
            }
        }
    }
}