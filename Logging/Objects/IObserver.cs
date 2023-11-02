namespace Logging.Objects;
public abstract class Observer : IObserver
{
    public Observer(ILog log) => log.Attach(this);
    public abstract Task Write(string serializedLog);
}
public interface IObserver
{
    public Task Write(string serializedLog);
}
