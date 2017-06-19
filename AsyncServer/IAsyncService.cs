namespace AsyncServer
{
    public interface IAsyncService
    {
        void Run();
        void Stop();
    }
}