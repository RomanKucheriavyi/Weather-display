using System.Linq;
using Abstract.Helpful.Lib;
using HomeServerApi.Logic;
using Microsoft.AspNetCore.Mvc;

namespace HomeServerApi.Controllers
{
    [ApiController]
    public sealed class ApiController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        private readonly DisplayOutput _noDataDisplayOutput = new("NO", "DATA");
        private readonly DisplayOutput _aboutDisplayOutput = new("API IS WORKING", "HELLO POSONY");

        private readonly WeatherFormatVersion _maxVersion;
        private readonly WeatherFormatVersion _minVersion;
        
        public ApiController(WeatherService weatherService)
        {
            _weatherService = weatherService;
            _minVersion = _weatherService.DisplayOutput.Select(pair => pair.Key).Min();
            _maxVersion = _weatherService.DisplayOutput.Select(pair => pair.Key).Max();
        }

        [HttpGet("weather")]
        public string GetWhether([FromQuery] int v)
        {
            if (v < _minVersion.Version || v > _minVersion.Version)
                v = _maxVersion.Version;
            
            var requestVersion = WeatherFormatVersion.From((byte) v);
            
            return _weatherService.DisplayOutput
                .GetOrDefaultValue(requestVersion)
                .ReplaceIfDefault(_noDataDisplayOutput)
                .ToSingleString();
        }
        
        [HttpGet("about")]
        public string GetAbout()
        {
            return _aboutDisplayOutput.ToSingleString();
        }
    }
}