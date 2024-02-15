using Zord.Extensions;

namespace UnitTests.ExtensionsTests
{
    public class EnumTests
    {
        [Fact]
        public void Should_Correct_Values()
        {
            var enumDesciption1 = TestEnum.Value1.GetDescription();
            enumDesciption1.Should().Be("Description 1");

            var enumNameOfDisplay1 = TestEnum.Value1.GetNameOfDisplay();
            enumNameOfDisplay1.Should().Be("Name of Display 1");

            var enumDescriptionOfDisplay1 = TestEnum.Value1.GetDescriptionOfDisplay();
            enumDescriptionOfDisplay1.Should().Be("Description of Display 1");

            var enumOptions = EnumHelper.GetOptions<TestEnum>();
            foreach (var option in enumOptions)
            {
                option.StringValue.Should().Be($"Value{option.Value}");

                option.Description.Should().Be($"Description {option.Value}");
            }
        }
    }
}