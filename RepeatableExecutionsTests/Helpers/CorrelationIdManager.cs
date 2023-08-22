namespace RepeatableExecutionsTests.Helpers
{
    public class CorrelationIdManager
    {
        private static AsyncLocal<Guid> correlationId = new AsyncLocal<Guid>();

        public static void Initialize()
        {
            correlationId.Value = Guid.NewGuid();
        }

        public static Guid GetCurrentCorrelationId()
        {
            return correlationId.Value;
        }
    }
}
