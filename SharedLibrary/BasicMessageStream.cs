using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary.Messages;

namespace SharedLibrary
{
    public class BasicMessageStream : IMessageStream
    {
        private const byte _etb = 23;
        private const int _bufferSize = 1024;
        private BlockingCollection<Message> messages = new BlockingCollection<Message>();
        private Stream stream;
        private CancellationTokenSource cancellation = new CancellationTokenSource();
        public bool IsListening { get; private set; } = false;
        public void Stop()
        {
            cancellation.Cancel();
        }
        public async void StartListeningAsync()
        {
            IsListening = true;
            byte[] buffer = new byte[_bufferSize];
            MemoryStream loadedBytes = new MemoryStream();
            while (!cancellation.IsCancellationRequested)
            {
                int read;
                try
                {
                    read = await stream.ReadAsync(buffer, 0, _bufferSize);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception interrupt reading from stream.\n{e.Message}");
                    break;
                }
                if (read == 0)
                {
                    Console.WriteLine("Client disconnected from stream.");
                    break;
                }
                loadedBytes.Write(buffer, 0, read);
                byte[] streamContent = loadedBytes.ToArray();
                int index;
                int lastPosition = 0;
                while ((index = Array.FindIndex(streamContent, lastPosition, @byte => @byte == _etb)) >= 0)
                {
                    byte[] rawMessage = new byte[index - lastPosition];
                    Array.Copy(streamContent, rawMessage, rawMessage.Length);
                    Message message = SerializerHelper.Deserialize(rawMessage);
                    if (message != null)
                    {
                        message.Sender = this;
                        messages.Add(message);
                    }
                    lastPosition = index + 1;
                }
                loadedBytes = new MemoryStream();
                loadedBytes.Write(streamContent, lastPosition, streamContent.Length - lastPosition);
            }
            IsListening = false;
            cancellation.Cancel();
        }
        public BasicMessageStream(Stream stream)
        {
            this.stream = stream;
        }
        public Task<Message> GetMessageAsync(CancellationToken token)
        {
            CancellationToken newToken = CancellationTokenSource.CreateLinkedTokenSource(token, cancellation.Token).Token;
            return Task.Run(() => messages.Take(newToken));
        }

        public async Task SendMessageAsync(Message message)
        {
            byte[] rawMessage = SerializerHelper.Serialize(message);
            await stream.WriteAsync(rawMessage, 0, rawMessage.Length);
        }
    }
}