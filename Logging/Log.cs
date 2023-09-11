using System.Text.Json;

namespace Logging
{
    public class Log : ILog
    {
        public IList<LogObject> Interactions { get; private set; } = new List<LogObject>();
        public LogObject Exit { get; private set; }

        public void AddInteraction(LogObject interaction)
        {
            Interactions.Add(interaction);
        }
        public void LogExit(LogObject exit)
        {
            Exit = exit;
        }
        public void WriteToFile()
        {
            string folderPath = "../logs";
            string serializedLog = JsonSerializer.Serialize(this);
            var guid = Guid.NewGuid().ToString();
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            File.WriteAllText($"{folderPath}/{guid}.txt", serializedLog);
        }
    }
    public interface ILog
    {
        public void AddInteraction(LogObject interaction);
        public void LogExit(LogObject exit);
        public void WriteToFile();
    }
}
