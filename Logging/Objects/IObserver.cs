namespace Logging.Objects
{
    public interface IObserver
    {
        void Update(ILog subject);
    }
}
