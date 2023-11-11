using Logging.Logging.Objects;
using Microsoft.Extensions.DependencyInjection;

namespace Logging.Logging.Configurations;
public class WriterConfigurations
{
    private readonly LoggerConfiguration _config;
    public static string FilePath { get; private set; } = ".";
    public static bool IsWritingToFile { get; private set; } = false;
    private readonly IServiceCollection _serviceCollection;
    private static readonly List<Type> CustomSinks = new();
    public WriterConfigurations(LoggerConfiguration config, IServiceCollection serviceCollection)
    {
        _config = config;
        _serviceCollection = serviceCollection;
    }
    public LoggerConfiguration CustomSink<TSink>() where TSink : class, IObserver
    {
        CustomSinks.Add(typeof(TSink));
        _serviceCollection.AddScoped<TSink>();
        return _config;
    }
    public static List<Type> GetRegisteredTypes()
    {
        return CustomSinks;
    }
    public LoggerConfiguration File(string filePath = ".")
    {
        IsWritingToFile = true;
        FilePath = filePath;
        return _config;
    }
}
