using Microsoft.Extensions.Options;

namespace Host.TestOption
{
    public class TestOptions
    {
        public string Name { get; set; } = "Test";
        public string Description { get; set; } = "Test description";
    }

    public class CustomTestOptions : IConfigureOptions<TestOptions>
    {
        public void Configure(TestOptions options)
        {
            options.Name = "Custom test";
            options.Description = "Custom test description";
        }
    }
}