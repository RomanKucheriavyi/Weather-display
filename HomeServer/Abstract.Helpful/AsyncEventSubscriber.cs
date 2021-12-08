using System;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public sealed class AsyncEventSubscriber
    {
        private readonly Func<Task> _asyncAction;

        public AsyncEventSubscriber(Func<Task> asyncAction)
        {
            _asyncAction = asyncAction;
        }
        
        public Task InvokeAsync()
        {
            return _asyncAction();
        }
    }
    
    public sealed class AsyncEventSubscriber<TArgs>
    {
        private readonly Func<TArgs, Task> _asyncAction;

        public AsyncEventSubscriber(Func<TArgs, Task> asyncAction)
        {
            _asyncAction = asyncAction;
        }
        
        public Task InvokeAsync(TArgs args)
        {
            return _asyncAction(args);
        }
    }
}