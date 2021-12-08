using FluentAssertions;
using HomeServerApi.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeServerApiTests
{
    [TestClass]
    public sealed class DisplayFormatTests
    {
        [TestMethod]
        public void TestFormatV2()
        {
            var display1 = WeatherService.FormatV2(new WeatherService.DisplayArgs(-24, 100, 10.645789f));
            display1.InvariantTest();
            display1.Line1.Should().Be("T:   H:   W:    ");
            display1.Line2.Should().Be("-24C 100% 11m/s ");
            //                          1234567890123456

            var display2 = WeatherService.FormatV2(new WeatherService.DisplayArgs(0, 0, 9.68f));
            display2.InvariantTest();
            display2.Line1.Should().Be("T:   H:   W:    ");
            display2.Line2.Should().Be("0C   0%   9.7m/s");
            //                          1234567890123456
        }

        [TestMethod]
        public void TestFormatV1()
        {
            var display1 = WeatherService.FormatV1(new WeatherService.DisplayArgs(-24, 100, 10.645789f));
            var display2 = WeatherService.FormatV1(new WeatherService.DisplayArgs(0, 0, 9.68f));
            display1.InvariantTest();
            display2.InvariantTest();
        }
    }
}