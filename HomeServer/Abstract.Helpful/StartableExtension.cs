using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Abstract.Helpful.Lib.Logging;
using Abstract.Helpful.Lib.Utils;
using Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Abstract.Helpful.Lib
{
    public sealed class TestLogger : ILogger
    {
        public Task LogAsync(LogText logText, LogChannelBase logChannel, LogEnvironment logEnvironment = LogEnvironment.Any,
            LogType logType = LogType.Information)
        {
            Console.WriteLine($"{DateTimeService.UtcNow:G} {logType,-11} : {logText}");
            return Task.CompletedTask;
        }
    }
    
    public static class StartableExtension
    {
        public static Task StartAllStartable(this IServiceProvider serviceProvider)
        {
            return StartAllStartable(
                serviceProvider.GetService<ILogger>(),
                serviceProvider.GetServices<IStartableMarker>(),
                serviceProvider.GetServices<IStartableAsyncMarker>()
            );
        }
        
        public static Task StartAllStartable(this IContainer container)
        {
            return StartAllStartable(
                container.Resolve<ILogger>(),
                container.Resolve<IEnumerable<IStartableMarker>>(),
                container.Resolve<IEnumerable<IStartableAsyncMarker>>()
            );
        }
        
        private static async Task StartAllStartable(ILogger logger, IEnumerable<IStartableMarker> startables, 
            IEnumerable<IStartableAsyncMarker> startablesAsync)
        {
            logger.LogLine();
            logger.LogToConsole("Start()");
            logger.LogToConsole("{");

            foreach (var startable in startables)
            {
                logger.LogToConsole($"{startable.GetType().Name} starting...");
                startable.Start();
            }

            foreach (var startableAsync in startablesAsync)
            {
                logger.LogToConsole($"{startableAsync.GetType().Name} starting...");
                await startableAsync.StartAsync();
            }
            
            logger.LogToConsole("}");
            logger.LogLine();
        }
    }
}