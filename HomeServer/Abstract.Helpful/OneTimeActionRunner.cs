using System;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public sealed class OneTimeActionRunner
    {
        private readonly AsyncLock _lock = new();
        private bool _isRunnedOnce = false;

        public Task RunOnceAsync(Func<Task> actionAsync)
        {
            return _lock.ExecuteThreadSafe(async () =>
            {
                if (_isRunnedOnce)
                    return;
                
                await actionAsync();

                _isRunnedOnce = true;
            });
        }
    }
}