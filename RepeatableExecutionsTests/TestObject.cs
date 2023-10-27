namespace RepeatableExecutionsTests
{
    public class TestObject
    {
        public TestObject2 obj2 { get; set; } = new TestObject2();
        public string name { get; set; } = string.Empty;
        public string lastName { get; set; }
        public int age { get; set; }
        public override string ToString() => name + " " + lastName + " " + age + " obj2: " + obj2;
    }
}
