using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstract.Helpful.Lib.Logging;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Abstract.Helpful.Lib
{
    public static class ServiceProviderExtension
    {
        private static bool isDisposed = false;

        public static Task DisposeAll(this IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger>();
            var disposables = serviceProvider.GetServices<IDisposable>();
            var disposablesAsync = serviceProvider.GetServices<IDisposableAsync>();
            return DisposeAll(logger, disposables, disposablesAsync);
        }
        
        public static Task DisposeAll(this Autofac.IContainer container)
        {
            var logger = container.Resolve<ILogger>();
            var disposables = container.Resolve<IEnumerable<IDisposable>>();
            var disposablesAsync = container.Resolve<IEnumerable<IDisposableAsync>>();
            return DisposeAll(logger, disposables, disposablesAsync);
        }
        
        public static async Task DisposeAll(ILogger logger,
            IEnumerable<IDisposable> disposables, 
            IEnumerable<IDisposableAsync> disposablesAsync)
        {
            if (isDisposed)
                return;

            isDisposed = true;

            logger.LogLine();
            logger.LogToConsole("Dispose()");
            logger.LogToConsole("{");
            var exceptionsCount = 0;
            foreach (var disposable in disposables)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception exception)
                {
                    logger.LogToConsole($"\tERROR: On Dispose exception! Details: {exception}");
                    exceptionsCount++;
                }
            }
            foreach (var disposableAsync in disposablesAsync)
            {
                try
                {
                    await disposableAsync.DisposeAsync();
                }
                catch (Exception exception)
                {
                    logger.LogToConsole($"\tERROR: On Dispose exception! Details: {exception}");
                    exceptionsCount++;
                }
            }
            logger.LogToConsole("}");
            logger.LogToConsole($"Dispose completed. Exceptions: {exceptionsCount}");
            logger.LogLine();
        }
    }
}