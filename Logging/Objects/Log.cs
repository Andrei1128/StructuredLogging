using Newtonsoft.Json;

namespace Logging.Objects
{
    public class Log : ILog
    {
        public LogEntry Entry { get; set; }
        public LogExit Exit { get; set; }
        public List<Log> Interactions { get; set; } = new List<Log>();
        public void LogEntry(LogEntry entry) => Entry = entry;
        public void LogExit(LogExit exit) => Exit = exit;
        public void LogInteraction(Log interaction) => Interactions.Add(interaction);
        public void WriteToFile()
        {
            string folderPath = "../Logging/logs";
            string serializedLog = JsonConvert.SerializeObject(this);
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            File.WriteAllText($"{folderPath}/{guid}.json", serializedLog);
        }
    }
    public interface ILog
    {
        public void LogEntry(LogEntry entry);
        public void LogExit(LogExit exit);
        public void LogInteraction(Log interaction);
        public void WriteToFile();
    }
}