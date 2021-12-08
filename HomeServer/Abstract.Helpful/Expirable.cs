using System;
using Abstract.Helpful.Lib.Utils;

namespace Abstract.Helpful.Lib
{
    public sealed class Expirable<T>
    {
        public DateTime LastUpdate { get; private set; }
        public T Data { get; private set; }

        public Expirable(T data)
        {
            Data = data;
            LastUpdate = DateTimeService.UtcNow;
        }
        
        public bool IsExpired(TimeSpan expirationTime)
        {
            return LastUpdate.IsExpired(expirationTime);
        }

        public Expirable<T> Update(T actualData)
        {
            Data = actualData;
            LastUpdate = DateTimeService.UtcNow;
            return this;
        }
    }
}