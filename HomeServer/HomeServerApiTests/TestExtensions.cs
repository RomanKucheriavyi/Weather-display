using FluentAssertions;
using HomeServerApi.Logic;

namespace HomeServerApiTests
{
    public static class TestExtensions
    {
        public static void InvariantTest(this DisplayOutput displayOutput)
        {
            displayOutput.Line1.Length.Should().Be(DisplayOutput.ROW_LENGTH);
            displayOutput.Line2.Length.Should().Be(DisplayOutput.ROW_LENGTH);
        }
    }
}