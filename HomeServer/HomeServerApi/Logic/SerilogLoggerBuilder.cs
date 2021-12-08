using Abstract.Helpful.Lib;
using Autofac;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Extensions.Autofac.DependencyInjection;

namespace HomeServerApi.Logic
{
    public static class SerilogLoggerBuilder
    {
        public static void ServerRegisterSerilog(this ContainerBuilder builder)
        {
            var loggerConfiguration = BuildConfiguration();
            builder.RegisterSerilog(loggerConfiguration);
            builder.InstanceAsSelfAsImplementedInterfacesSingle(Log.Logger);
        }

        private static LoggerConfiguration BuildConfiguration()
        {
            var loggerConfiguration = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .MinimumLevel.Is(LogEventLevel.Information)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning)
                ;

            return loggerConfiguration;
        }
    }
}