namespace Logging.Objects;
public interface IObserver
{
    void Write(ILog subject);
}
