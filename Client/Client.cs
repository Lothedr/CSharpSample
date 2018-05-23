using System;
using System.Threading;
using SharedLibrary;
using SharedLibrary.Messages;

namespace Client
{
    public class Client
    {
        private IMessageStream messageStream;
        private CancellationTokenSource cancellation = new CancellationTokenSource();
        private ConsolePlus console;
        public void Stop()
        {
            cancellation.Cancel();
        }
        public Client(IMessageStream messageStream)
        {
            this.messageStream = messageStream;
        }
        public void Start()
        {
            console = new ConsolePlus(messageStream);
            console.Print("Connected.");
            console.Print("Simple guide:");
            console.Print("To start chat you must first register on server.");
            console.Print("Type /register NAME with your chosen name.");
            console.Print("After registration you can join to any chat by typing /join CHATNAME.");
            console.Print("When in chat, type anything not starting with '/' to send message.");
            console.Print("Type /exit to quit.");
            console.Print("Enjoy!");
            messageStream.StartListeningAsync();
            StartListeningAsync();
            console.StartReading();
        }
        private async void StartListeningAsync()
        {
            while (!cancellation.IsCancellationRequested)
            {
                try
                {
                    Message message = await messageStream.GetMessageAsync(cancellation.Token);
                    message.UpdateClient(console);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
            messageStream.Stop();
        }
    }
}