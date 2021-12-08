using System;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public static class AsyncEventExtension 
    {
        public static void Subscribe(this IAsyncEvent asyncEvent, Func<Task> asyncAction)
        {
            asyncEvent.Subscribe(new AsyncEventSubscriber(asyncAction));
        }
        
        public static void Subscribe<TArgs>(this IAsyncEvent<TArgs> asyncEvent, Func<TArgs, Task> asyncAction)
        {
            asyncEvent.Subscribe(new AsyncEventSubscriber<TArgs>(asyncAction));
        }
    }
}