using System;

namespace Abstract.Helpful.Lib.Utils
{
    public sealed class DisposableAction : IDisposable
    {
        private readonly Action _dispose;
        private bool _isDisposed = false;

        public DisposableAction(Action dispose)
        {
            _dispose = dispose;
        }

        public static DisposableAction Empty()
        {
            return new(null);
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            
            _dispose?.Invoke();
        }
    }
}