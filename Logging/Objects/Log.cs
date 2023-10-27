using Logging.Configurations;
using Newtonsoft.Json;

namespace Logging.Objects;
public class Log : ILog
{
    public LogEntry Entry { get; set; }
    public LogExit Exit { get; set; }
    public List<string> Infos { get; } = new List<string>();
    public List<Log> Interactions { get; } = new List<Log>();
    private readonly IList<IObserver> _observers = new List<IObserver>();
    public void AddInformation(string info) => Infos.Add(info);
    public void LogEntry(LogEntry entry) => Entry = entry;
    public void LogExit(LogExit exit) => Exit = exit;
    public void AddInteraction(Log interaction) => Interactions.Add(interaction);
    public void Write()
    {
        string? serializedLog = null;
        if (WriterConfigurations.IsWritingToConsole)
        {
            serializedLog ??= JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            Console.WriteLine(serializedLog);
        }
        if (WriterConfigurations.IsWritingToFile)
        {
            serializedLog ??= JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            string filePath = WriterConfigurations.FilePath;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            File.WriteAllText($"{filePath}\\{WriterConfigurations.FileName}", serializedLog);
        }
        Notify();
    }
    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify()
    {
        foreach (IObserver observer in _observers)
            observer.Write(this);
    }
}
public interface ILog
{
    public void AddInformation(string info);
    public void LogEntry(LogEntry entry);
    public void LogExit(LogExit exit);
    public void AddInteraction(Log interaction);
    public void Write();
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}