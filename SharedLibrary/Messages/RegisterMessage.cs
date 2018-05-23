using System;
using System.Linq;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class RegisterMessage : Message
    {
        public string Name { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            lock (serverKnowledge)
            {
                if (serverKnowledge.ClientToName.TryGetValue(Sender, out string currentName))
                {
                    Sender.SendMessageAsync(new ErrorMessage
                    {
                        Error = $"You are already registered with name \"{currentName}\"."
                    });
                }
                else
                {
                    if (serverKnowledge.ClientToName.Values.Contains(Name))
                    {
                        Sender.SendMessageAsync(new ErrorMessage
                        {
                            Error = $"Name \"{Name}\" is already taken."
                        });
                    }
                    else
                    {
                        serverKnowledge.ClientToName.Add(Sender, Name);
                        Sender.SendMessageAsync(new ConfirmMessage());
                        Console.WriteLine($"New client with name {Name}.");
                    }
                }
            }
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.Print($"Unsupported message {nameof(RegisterMessage)}.");
        }
    }
}