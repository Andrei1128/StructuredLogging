using StructuredLogging.Helpers;

namespace StructuredLogging.Service
{
    public static class ProxyDecorator
    {
        public static void ExecuteWithProxy(Action action)
        {
            Console.WriteLine("Proxy: Before method call");
            Console.WriteLine(CorrelationIdManager.GetCurrentCorrelationId());
            action.Invoke();
            Console.WriteLine("Proxy: After method call");
        }
    }
}
