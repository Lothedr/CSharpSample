using SharedLibrary;

namespace Client
{
    public interface IConnectionModule
    {
        IMessageStream Connect();
    }
}