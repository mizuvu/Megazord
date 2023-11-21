using System.ComponentModel;

namespace UnitTests.ExtensionsTests
{
    public enum TestEnum
    {
        [Description("Description 1")]
        Value1 = 1,

        [Description("Description 2")]
        Value2 = 2,

        [Description("Description 3")]
        Value3 = 3,
    }
}
