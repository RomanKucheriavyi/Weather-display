using System;
using System.Linq;
using System.Threading.Tasks;
using Abstract.Helpful.Lib;
using FluentAssertions;
using HomeServerApi.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HomeServerApiTests
{
    [TestClass]
    public sealed class WeatherServiceTests
    {
        [TestMethod]
        public async Task API_ShouldWork()
        {
            var weatherService = new WeatherService(default);
            await weatherService.StartAsync();

            for (var i = 0; i < 10; i++)
            {
                if (!weatherService.DisplayOutput[WeatherFormatVersion.From(1)].IsDefault())
                    break;
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            
            weatherService.DisplayOutput[WeatherFormatVersion.From(1)].InvariantTest();
            weatherService.DisplayOutput[WeatherFormatVersion.From(2)].InvariantTest();
            
            weatherService.DisplayOutput[WeatherFormatVersion.From(1)].Line1.First().Should().NotBe(' ');
            weatherService.DisplayOutput[WeatherFormatVersion.From(2)].Line1.First().Should().NotBe(' ');
        }
    }
}