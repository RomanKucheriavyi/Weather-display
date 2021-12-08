using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public sealed class ExpirableConcurrentDictionary<TKey, TValue> : IDisposable
    {
        private readonly TimeSpan _expirationTime;

        private readonly ConcurrentDictionary<TKey, Expirable<TValue>> _dictionary =
            new();

        private CancellationTokenSource _cancellationTokenSource = new();
        
        [Safe]
        public ExpirableConcurrentDictionary(TimeSpan expirationTime)
        {
            _expirationTime = expirationTime;

            Task.Factory.StartNew(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    await Task.Delay(_expirationTime);
                    RemoveExpiredValues();
                }
            }).SyncWait();
        }

        [Safe]
        private void RemoveExpiredValues()
        {
            var keys = _dictionary.Keys.ToList();
            foreach (var key in keys)
            {
                if (_dictionary.TryGetValue(key, out var value) &&
                    value.IsExpired(_expirationTime))
                {
                    _dictionary.TryRemove(key, out _);
                }
            }
        }
        
        [Safe]
        public void AddOrUpdate(TKey key, TValue value)
        {
            _dictionary.AddOrUpdate(key, new Expirable<TValue>(value),
                (_, expirable) => expirable.Update(value));
        }

        [Safe]
        public bool IsExpired(TKey key)
        {
            if (!_dictionary.TryGetValue(key, out var expirable))
                return true;

            if (expirable.IsExpired(_expirationTime))
            {
                _dictionary.TryRemove(key, out _);
                return true;
            }

            return false;
        }
        
        [Safe]
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (!_dictionary.TryGetValue(key, out var expirable))
            {
                value = default;
                return false;
            }

            if (expirable.IsExpired(_expirationTime))
            {
                _dictionary.TryRemove(key, out _);
                value = default;
                return false;
            }

            value = expirable.Data;
            return true;
        }

        [Safe]
        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = default;
            _dictionary.Clear();
        }
    }
}