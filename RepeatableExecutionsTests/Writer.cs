using Logging.Objects;
using Newtonsoft.Json;

namespace RepeatableExecutionsTests
{
    public class Writer : IObserver
    {
        public Writer(ILog log) => log.Attach(this);
        public void Update(ILog subject)
        {
            string serializedLog = JsonConvert.SerializeObject(subject as Log);
            Console.WriteLine(serializedLog);
        }
    }
}
