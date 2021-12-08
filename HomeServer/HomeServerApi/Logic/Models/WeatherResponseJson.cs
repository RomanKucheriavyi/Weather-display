using System.Collections.Generic;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace HomeServerApi.Logic
{
    /// <summary>
    /// API object
    /// </summary>
    public sealed class WeatherResponseJson
    {
        public Data data { get; set; }
        
        public sealed class Data
        {
            public List<TimeLine> timelines { get; set; }
        }
        
        public sealed class TimeLine
        {
            public List<Interval> intervals { get; set; }
        }
        
        public sealed class Interval
        {
            public Values values { get; set; }
        }
        
        public sealed class Values
        {
            public double temperature { get; set; }
            public double humidity { get; set; }
            public double windSpeed { get; set; }
        }
    }
}