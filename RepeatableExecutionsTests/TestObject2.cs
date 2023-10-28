namespace RepeatableExecutionsTests
{
    public class TestObject2
    {
        public string name2 { get; set; } = "naspa";
        public int[] numbers { get; set; } = { 1, 2, 3 };
        public long age2 { get; set; } = 56;
        public override string ToString() => name2 + " " + age2;
    }
}
