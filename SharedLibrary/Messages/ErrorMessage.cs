using System;

namespace SharedLibrary.Messages
{
    [Serializable]
    public class ErrorMessage : Message
    {
        public ErrorMessage()
        {

        }
        public ErrorMessage(string error)
        {
            Error = error;
        }
        public string Error { get; set; }
        public override void UpdateServer(ServerKnowledge serverKnowledge)
        {
            Console.WriteLine($"Error message from client: {Error}");
        }

        public override void UpdateClient(ConsolePlus console)
        {
            console.Print($"Server error: {Error}");
        }
    }
}