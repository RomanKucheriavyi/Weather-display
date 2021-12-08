using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstract.Helpful.Lib.Utils;

namespace Abstract.Helpful.Lib
{
    public sealed class DelayOnDemand<TKey>
    {
        private readonly Dictionary<TKey, DateTime> _lastExecutionTimesPerKey = new();
        private readonly TimeSpan _timeRangeWhenDelaying;
        private readonly Func<TimeSpan> _delayFactory;
        
        public DelayOnDemand(TimeSpan timeRangeWhenDelaying, Func<TimeSpan> delayFactory)
        {
            _timeRangeWhenDelaying = timeRangeWhenDelaying;
            _delayFactory = delayFactory;
        }
        
        public async Task DelayIfNeeded(TKey key)
        {
            var isExecutedAnyTime = _lastExecutionTimesPerKey.TryGetValue(key, out var lastExecutionTime);
            var isDelayNeeded = isExecutedAnyTime && !lastExecutionTime.IsExpired(_timeRangeWhenDelaying);
            if (isDelayNeeded)
            {
                var delay = _delayFactory();
                await Task.Delay(delay);
            }
        }

        public void Executed(TKey key)
        {
            _lastExecutionTimesPerKey.AddOrUpdate(key, DateTimeService.UtcNow);
        }
    }
}