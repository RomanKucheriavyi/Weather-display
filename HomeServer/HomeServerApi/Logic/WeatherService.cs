using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abstract.Helpful.Lib;
using Newtonsoft.Json;
using Serilog;

namespace HomeServerApi.Logic
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class WeatherService : IDisposable, IStartableAsyncMarker
    {
        private const string API_REQUEST_URI = "https://data.climacell.co/v4/timelines?location=60562b0d5741510007743bed&fields=temperature&fields=humidity&fields=windSpeed&timesteps=current&units=metric&apikey=sO8I7b0gc7aZy3NZN6VKohnAyjlHKWIP";

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly TimeSpan _loopDelay  = TimeSpan.FromHours(0.5);
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        private readonly Dictionary<WeatherFormatVersion, Func<DisplayArgs, DisplayOutput>> _displayFormatters;

        public Dictionary<WeatherFormatVersion, DisplayOutput> DisplayOutput { get; }

        public WeatherService(ILogger logger)
        {
            _logger = logger;
            
            _displayFormatters = new()
            {
                { WeatherFormatVersion.From(1), FormatV1 },
                { WeatherFormatVersion.From(2), FormatV2 },
            };
            
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
            };
            _httpClient = new HttpClient(handler);

            DisplayOutput = _displayFormatters.ToDictionary(
                pair => pair.Key, 
                _ => default(DisplayOutput));
        }

        public async Task StartAsync()
        {
            await Task.Factory.StartNew(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        await Loop();
                    }
                    catch (Exception exception)
                    {
                        _logger?.Error(exception, $"{nameof(Loop)} failed");
                    }
                    await Task.Delay(_loopDelay);
                }
            }, TaskCreationOptions.LongRunning);
        }

        private async Task Loop()
        {
            using var response = await _httpClient.GetAsync(API_REQUEST_URI, HttpCompletionOption.ResponseContentRead);
            using var content = response.Content;
            var contentAsStr = await content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<WeatherResponseJson>(contentAsStr);
            var values = responseJson.data.timelines.First().intervals.First().values;
            var temperature = values.temperature.RoundToInt();
            var humidity = values.humidity.RoundToInt();
            var windSpeed = (values.windSpeed * 10).RoundToInt() / 10.0f;
            var displayArgs = new DisplayArgs(temperature, humidity, windSpeed);

            foreach (var pair in _displayFormatters)
            {
                var displayOutput = pair.Value(displayArgs);
                
                DisplayOutput[pair.Key] = displayOutput;

                _logger?.Information($"Weather updated:{Environment.NewLine}{displayOutput}");
            }
        }
        
        public static DisplayOutput FormatV2(DisplayArgs args)
        {
            var stringBuilder = new StringBuilder();

            if (args.IsTemperatureValid())
                stringBuilder.Append(args.Temperature).Append("C");
            else
                stringBuilder.Append("N/A");
            stringBuilder.EnsureHasSameLength("T:   ");
            
            if (args.IsHumidityValid())
                stringBuilder.Append(args.Humidity).Append("%");
            else
                stringBuilder.Append("N/A");
            stringBuilder.EnsureHasSameLength("T:   H:   ");

            if (args.IsWindSpeedValid())
            {
                var windSpeedFormat = args.WindSpeed >= 10 ? "F0" : "F1";
                stringBuilder.Append(args.WindSpeed.ToString(windSpeedFormat)).Append("m/s");
            }
            else
                stringBuilder.Append("N/A");
            
            return new DisplayOutput("T:   H:   W:", stringBuilder.ToString());
        }

        public static DisplayOutput FormatV1(DisplayArgs args)
        {
            var line1 = "T: ";
            var line1Ending = ":W";
            if (args.IsTemperatureValid())
            {
                line1 += args.Temperature;
                line1 += "C";
            }
            else
            {
                line1 += "N/A";
            }
            line1 += new string(' ', Logic.DisplayOutput.ROW_LENGTH - line1.Length - line1Ending.Length);
            line1 += line1Ending;

            var line2 = "H: ";
            var isHumidityValid = args.IsHumidityValid();
            if (isHumidityValid)
            {
                line2 += args.Humidity;
                line2 += "%";
            }
            else
            {
                line2 += "N/A";
            }
            var line2Ending = "";
            var isWindSpeedValid = args.IsWindSpeedValid();
            if (isWindSpeedValid)
            {
                line2Ending += args.WindSpeed.ToString("F1");
                line2Ending += "m/s";
            }
            else
            {
                line2Ending += "N/A";
            }
            line2 += new string(' ', Logic.DisplayOutput.ROW_LENGTH - line2.Length - line2Ending.Length);
            line2 += line2Ending;

            return new DisplayOutput(line1, line2);
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
        }
        
        public readonly struct DisplayArgs
        {
            public int Temperature { get; }
            public int Humidity { get; }
            public float WindSpeed { get; }

            public DisplayArgs(int temperature, int humidity, float windSpeed)
            {
                Temperature = temperature;
                Humidity = humidity;
                WindSpeed = windSpeed;
            }
            
            public bool IsTemperatureValid()
            {
                return Temperature > -100 && Temperature < 100;
            }

            public bool IsHumidityValid()
            {
                return Humidity >= 0 && Humidity <= 100;
            }
            
            public bool IsWindSpeedValid()
            {
                return WindSpeed >= 0 && WindSpeed <= 100;
            }
        }
    }
}