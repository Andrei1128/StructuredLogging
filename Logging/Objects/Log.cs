using Logging.Configurations;
using Newtonsoft.Json;

namespace Logging.Objects
{
    public class Log : ILog
    {
        public LogEntry Entry { get; set; }
        public LogExit Exit { get; set; }
        public List<string> Infos { get; } = new List<string>();
        public List<Log> Interactions { get; } = new List<Log>();
        public void AddInformation(string info) => Infos.Add(info);
        public void LogEntry(LogEntry entry) => Entry = entry;
        public void LogExit(LogExit exit) => Exit = exit;
        public void AddInteraction(Log interaction) => Interactions.Add(interaction);
        public void Write()
        {
            string serializedLog = JsonConvert.SerializeObject(this);
            if (WriterConfigurations.IsWritingToFile)
            {
                string filePath = WriterConfigurations.FilePath;
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                File.WriteAllText($"{filePath}\\{WriterConfigurations.FileName}", serializedLog);
            }
        }
    }
    public interface ILog
    {
        public void AddInformation(string info);
        public void LogEntry(LogEntry entry);
        public void LogExit(LogExit exit);
        public void AddInteraction(Log interaction);
        public void Write();
    }
}