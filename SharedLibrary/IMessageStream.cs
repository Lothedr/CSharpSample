using System.Threading;
using System.Threading.Tasks;
using SharedLibrary.Messages;

namespace SharedLibrary
{
    public interface IMessageStream
    {
        void StartListeningAsync();
        void Stop();
        Task<Message> GetMessageAsync(CancellationToken token);
        Task SendMessageAsync(Message message);
    }
}