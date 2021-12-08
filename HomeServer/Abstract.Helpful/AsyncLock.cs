using System;
using System.Threading;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    /// <summary>
    ///     lock(){} for async methods
    /// </summary>
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        public async Task ExecuteThreadSafe(Func<Task> actionAsync)
        {
            try
            {
                await _semaphore.WaitAsync();
                await actionAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        
        public async Task<T> ExecuteThreadSafe<T>(Func<Task<T>> actionAsync)
        {
            try
            {
                await _semaphore.WaitAsync();
                return await actionAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }
    }
}