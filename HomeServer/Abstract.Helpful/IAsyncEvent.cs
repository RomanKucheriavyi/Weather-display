using System;

namespace Abstract.Helpful.Lib
{
    public interface IAsyncEvent
    {
        IDisposable Subscribe(AsyncEventSubscriber subscriber);
    }
    
    public interface IAsyncEvent<TArgs>
    {
        IDisposable Subscribe(AsyncEventSubscriber<TArgs> subscriber);
    }
}