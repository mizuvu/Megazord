namespace Host.Models
{
    public class TestModel
    {
        public int Id { get; set; } = 1;
        public string? Name { get; set; } = $"Name at {DateTime.Now}";
    }
}
