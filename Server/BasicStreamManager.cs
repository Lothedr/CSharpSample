using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharedLibrary;
using SharedLibrary.Messages;

namespace Server
{
    public class BasicStreamManager : IStreamManager
    {
        private readonly ServerKnowledge knowledge;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();

        public void Stop()
        {
            cancellation.Cancel();
        }
        public BasicStreamManager(ServerKnowledge knowledge)
        {
            this.knowledge = knowledge;
        }
        public async Task ServeStreamAsync(Stream stream)
        {
            BasicMessageStream basicMessageStream = new BasicMessageStream(stream);
            basicMessageStream.StartListeningAsync();
            while (!cancellation.IsCancellationRequested)
            {
                try
                {
                    Message message = await basicMessageStream.GetMessageAsync(cancellation.Token);
                    message.UpdateServer(knowledge);
                }
                catch (OperationCanceledException)
                {
                    basicMessageStream.Stop();
                    break;
                }
            }
            knowledge.ClientDisconnected(basicMessageStream);
        }
    }
}