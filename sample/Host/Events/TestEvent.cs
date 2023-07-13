namespace Host.Events
{
    public class TestEvent
    {
        public int Id { get; set; } = 1;
        public string? Name { get; set; } = $"Name at {DateTime.Now}";
    }
}
