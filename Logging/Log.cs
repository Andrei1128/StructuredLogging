using System.Text.Json;

namespace Logging
{
    public class Log : ILog
    {
        public LogObject Entry { get; set; }
        public IList<Log> Interactions { get; private set; } = new List<Log>();
        public void LogEntry(LogObject entry)
        {
            Entry = entry;
        }
        public void LogInteraction(Log interaction)
        {
            Interactions.Add(interaction);
        }
        public void WriteToFile()
        {
            string folderPath = "../Logging/logs";
            string serializedLog = JsonSerializer.Serialize(this);
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            File.WriteAllText($"{folderPath}/{guid}.json", serializedLog);
        }
    }
    public interface ILog
    {
        public void LogEntry(LogObject entry);
        public void LogInteraction(Log interaction);
        public void WriteToFile();
    }
}