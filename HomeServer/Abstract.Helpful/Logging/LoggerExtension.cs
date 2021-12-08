using System.Threading.Tasks;

namespace Abstract.Helpful.Lib.Logging
{
    public static class LoggerExtension
    {
        public static void LogStarting(this ILogger logger, string className)
        {
            logger.LogToConsole($"{className} starting");
        }

        public static void LogDisposing(this ILogger logger, string className)
        {
            logger.LogToConsole($"{className} disposing");
        }
        
        public static void LogLine(this ILogger logger)
        {
            logger.LogToConsole(new string('-', 50));
        } 
        
        public static void LogToConsole(this ILogger logger,
            LogText logText, 
            LogType logType = LogType.Information,
            LogEnvironment logEnvironment = LogEnvironment.Any)
        {
            logger.LogToConsoleAsync(logText, logType, logEnvironment).GetAwaiter().GetResult();
        }
        
        public static Task LogToConsoleAsync(this ILogger logger,
            LogText logText, 
            LogType logType = LogType.Information,
            LogEnvironment logEnvironment = LogEnvironment.Any)
        {
            return logger.LogAsync(logText, ConsoleLogChannel.Instance, logEnvironment, logType);
        }
        
        public static Task LogToStorageAsync(this ILogger logger,
            LogText logText, 
            LogType logType = LogType.Information,
            LogEnvironment logEnvironment = LogEnvironment.Any)
        {
            return logger.LogAsync(logText, StorageLogChannel.Instance, logEnvironment, logType);
        }
    }
}