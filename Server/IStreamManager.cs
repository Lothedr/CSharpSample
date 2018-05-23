using System.IO;
using System.Threading.Tasks;

namespace Server
{
    public interface IStreamManager
    {
        void Stop();
        Task ServeStreamAsync(Stream stream);
    }
}