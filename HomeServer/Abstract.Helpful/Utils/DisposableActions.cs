using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Abstract.Helpful.Lib.Utils
{
    public sealed class DisposableActions : IDisposable
    {
        private readonly List<Action> _disposeActions = new();
        private bool _isDisposed = false;

        public DisposableActions()
        {
        }

        public void Add(Action disposeAction)
        {
            _disposeActions.Add(disposeAction);
        }
        
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            
            var exceptions = new List<Exception>();
            foreach (var disposeAction in _disposeActions)
            {
                try
                {
                    disposeAction();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
            if (exceptions.Count > 0)
                Debug.WriteLine($"DisposableActions: Exceptions: {exceptions.ToPrettyString()}");
        }
    }
}