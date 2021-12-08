using System;
using System.Threading;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib.ProgramFeatures
{
    public class ConsoleTitleUpdater : IStartableMarker, IDisposable
    {
        private readonly CancellationTokenSource _cancellationToken = new();
        private readonly TimeSpan _titleUpdateDelay = TimeSpan.FromSeconds(1.0);
        protected virtual string ServiceName { get; } = "";
        protected virtual int PredefinedWidth { get; } = 220;
        protected virtual int PredefinedHeight { get; } = 1200;

        public ConsoleTitleUpdater(string serviceName = "")
        {
            ServiceName = serviceName;
        }

        private async Task UpdateTitle()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Colorful.Console.Title = $"{ServiceName} : Uptime / {ProgramUptime.FullUptime}";
                await Task.Delay(_titleUpdateDelay);
            }
        }

        public void Start()
        {
            try
            {
                Colorful.Console.SetBufferSize(PredefinedWidth, PredefinedHeight);
            }
            catch (Exception)
            {
            }

            Task.Factory.StartNew(async () => await UpdateTitle());
        }

        public void Dispose()
        {
            _cancellationToken.Cancel();
        }
    }
}