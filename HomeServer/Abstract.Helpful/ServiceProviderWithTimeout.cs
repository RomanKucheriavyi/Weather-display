using System;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public sealed class ServiceProviderWithTimeout : IServiceProvider
    {
        private readonly IServiceProvider _root;

        public ServiceProviderWithTimeout(IServiceProvider root)
        {
            _root = root;
        }
            
        public object GetService(Type serviceType)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    return _root.GetService(serviceType);
                }
                catch
                {
                    return null;
                }
            });
            
            if (task.Wait(TimeSpan.FromSeconds(10)))
                return task.Result;
            
            throw new TimeoutException($"Service Resolving Timed out, Type: {serviceType.Name}. " +
                                       $"95% That you forget to open Azure Emulator");
        }
    }

    public static class TimeoutAction
    {
        // TODO: Task is not killed, so solution is not the very useful
        public static bool Run(Action action, TimeSpan timeout)
        {
            Exception exception = default;
            
            var task = Task.Run(() =>
            {
                try
                {
                    action();
                    return 1;
                }
                catch (Exception actualException)
                {
                    exception = actualException;
                    return 1;
                }
            });

            if (task.Wait(timeout))
            {
                if (!exception.IsDefault())
                    exception.Rethrow();

                return true;
            }

            return false;
        }
    }
}