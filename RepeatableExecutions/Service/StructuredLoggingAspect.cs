using PostSharp.Aspects;
using RepeatableExecutions.Data.Entities;
using RepeatableExecutions.Data.ValueObjects;
using Serilog;

namespace RepeatableExecutions.Service
{
    public class StructuredLoggingAspect : OnMethodBoundaryAspect
    {
        private List<LogObject> _interactions;
        private ILogger _logger;

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            _interactions = new List<LogObject>();
            _logger = new LoggerConfiguration()
            .WriteTo.File("..\\log.txt", rollingInterval: RollingInterval.Minute)
            .CreateLogger();
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            var entry = new LogObject
            {
                Time = DateTimeOffset.Now,
                Operation = args.Method.Name,
                Input = args.Arguments.ToArray()
            };
            _interactions.Add(entry);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var exit = new LogObject
            {
                Time = DateTimeOffset.Now,
                Operation = null,
                Input = null,
                Output = new { StatusCode = 200 }
            };
            var fullLogEntry = new LogEntry
            {
                Entry = _interactions[0],
                Interactions = _interactions.GetRange(1, _interactions.Count - 1),
                Exit = exit
            };
            _logger.Information("{@LogEntry}", fullLogEntry);
        }
    }
}