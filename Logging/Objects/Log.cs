using System.Text.Json;

namespace Logging.Objects
{
    public class Log : ILog
    {
        public LogEntry Entry { get; set; }
        public IList<Log> Interactions { get; private set; } = new List<Log>();
        public LogExit Exit { get; private set; }
        public Log(LogEntry entry) { Entry = entry; }
        public void LogEntry(LogEntry entry)
        {
            Entry = entry;
        }
        public void LogExit(LogExit exit)
        {
            Exit = exit;
        }
        public void LogInteraction(Log interaction)
        {
            Interactions.Add(interaction);
            //this = interaction;
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
        public void LogEntry(LogEntry entry);
        public void LogInteraction(Log interaction);
        public void LogExit(LogExit exit);
        public void WriteToFile();
    }
}