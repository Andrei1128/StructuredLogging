using Logging.Logging.Configurations;
using Newtonsoft.Json;

namespace Logging.Logging.Objects;
public class Log : ILog
{
    public int Id { get; init; } = 0;
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
        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        string? serializedLog = JsonConvert.SerializeObject(this, jsonSettings); ;
        if (WriterConfigurations.IsWritingToFile)
        {
            string filePath = WriterConfigurations.FilePath;
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            File.WriteAllText($"{filePath}\\{Entry.Method}-{DateTime.Now:yyyyMMddHHmmssfffffff}", serializedLog);
        }
        Notify(serializedLog);
    }
    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);
    public void Notify(string serializedLog)
    {
        //Parallel.ForEach(_observers, (observer) =>
        //{
        //    observer.Write(serializedLog);
        //});
        foreach (var observer in _observers)
        {
            observer.Write(serializedLog);
        }
    }
}
public interface ILog : IObservable
{
    public void AddInformation(string info);
    public void LogEntry(LogEntry entry);
    public void LogExit(LogExit exit);
    public void AddInteraction(Log interaction);
    public void Write();

}
public interface IObservable
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify(string serializedLog);
}