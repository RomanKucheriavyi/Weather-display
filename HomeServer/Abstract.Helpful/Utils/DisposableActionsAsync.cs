using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib.Utils
{
    public sealed class DisposableActionsAsync : IDisposableAsync
    {
        private readonly List<Func<Task>> _disposeActions = new();
        private bool _isDisposed = false;

        public DisposableActionsAsync()
        {
        }

        public void Add(Func<Task> disposeActionAsync)
        {
            _disposeActions.Add(disposeActionAsync);
        }
        
        public async Task DisposeAsync()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            
            var exceptions = new List<Exception>();
            foreach (var disposeAction in _disposeActions)
            {
                try
                {
                    await disposeAction();
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