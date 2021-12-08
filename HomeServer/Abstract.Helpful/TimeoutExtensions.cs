using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public static class TimeoutExtensions
    {
        public static Task WithCustomTimeout(this Task task, TimeSpan timeout, bool isTestsEnvironment = false, 
            Action onTimeout = null)
        {
            if (isTestsEnvironment && Debugger.IsAttached)
                return task;
            
            var isTaskCompleted = task.Wait(timeout);

            if (!isTaskCompleted)
            {
                onTimeout?.Invoke();
                throw new TimeoutException($"Task timeout");
            }
                
            return task;
        }
        
        public static Task<T> WithCustomTimeout<T>(this Task<T> task, TimeSpan timeout, bool isTestsEnvironment = false)
        {
            if (isTestsEnvironment && Debugger.IsAttached)
                return task;
            
            var isTaskCompleted = task.Wait(timeout);

            if (!isTaskCompleted)
                throw new TimeoutException($"Task timeout");
                
            return task;
        }
        
        public static T WithCustomTimeout<T>(this Func<T> func, TimeSpan timeout, bool isTestsEnvironment = false)
        {
            if (isTestsEnvironment)
                return func();

            T result = default;
            var isTaskCompleted = Task.Factory.StartNew(() =>
            {
                result = func();
            }).Wait(timeout);

            if (!isTaskCompleted)
                throw new TimeoutException($"Task timeout");
                
            return result;
        }
        
        public static IServiceProvider WithCustomTimeout(this IServiceProvider root)
        {
            return new ServiceProviderWithTimeout(root);
        }
    }
}