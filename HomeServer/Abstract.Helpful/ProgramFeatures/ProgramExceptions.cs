using System;
using System.Threading.Tasks;
using Abstract.Helpful.Lib.Configs;
using Abstract.Helpful.Lib.Logging;

namespace Abstract.Helpful.Lib.ProgramFeatures
{
    public static class ProgramExceptions
    {
        public static ServiceName ServiceName { get; set; }
        public static EnvironmentValue EnvironmentValue { get; set; }
        
        private static string OptionalEnvironmentText
        {
            get
            {
                var serviceString = ServiceName.IsDefault() ? string.Empty : $"Service: {ServiceName}{Environment.NewLine}";
                var environmentString = EnvironmentValue.IsDefault() ? string.Empty : $"Environment: {EnvironmentValue}{Environment.NewLine}";
                
                return serviceString + environmentString + $"ComputerName: {Environment.MachineName.ToStringSafe()} ({Environment.UserName.ToStringSafe()}){Environment.NewLine}";
            }
        }
        
        public static void Catch_Unhandled_Exceptions(Action<object, UnhandledExceptionEventArgs> action)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                action(sender, args);
            };
        }

        public static void Catch_Unobserved_Exceptions(Action<object, UnobservedTaskExceptionEventArgs> action)
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                action(sender, args);
            };
        }

        public static void Catch_Unhandled_And_Unobserved_Exceptions_WithLogger(Func<ILogger> logger,
            LogChannelBase criticalLogChannel)
        {
            Catch_Unhandled_Exceptions((sender, args) =>
            {
                LogCritical(logger, $"Unhandled Exception{Environment.NewLine}{OptionalEnvironmentText}{args}", criticalLogChannel);
                LogCritical(logger, args.ExceptionObject as Exception, $"UnhandledException{Environment.NewLine}{OptionalEnvironmentText}", criticalLogChannel);
            });
            Catch_Unobserved_Exceptions((sender, args) =>
            {
                LogCritical(logger, args.Exception.Flatten(), $"UnobservedException{Environment.NewLine}{OptionalEnvironmentText}", criticalLogChannel);
                LogCritical(logger, args.Exception, $"UnobservedException{Environment.NewLine}{OptionalEnvironmentText}", criticalLogChannel);
            });
        }

        private static void LogCritical(Func<ILogger> logger, string text, LogChannelBase criticalLogChannel)
        {
            var loggerInstance = logger();

            loggerInstance?.LogAsync(text, criticalLogChannel, LogEnvironment.Any, LogType.Error)
                .GetAwaiter().GetResult();
        }
        
        private static void LogCritical(Func<ILogger> logger, Exception exception, string text,
            LogChannelBase criticalLogChannel)
        {
            var loggerInstance = logger();

            loggerInstance?.LogAsync(LogText.From(exception, text), criticalLogChannel, 
                    LogEnvironment.Any, LogType.Error)
                .GetAwaiter().GetResult();
        }
    }
}