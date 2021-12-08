using System;
using System.Reflection;
using System.Threading.Tasks;
using Abstract.Helpful.Lib;
using Abstract.Helpful.Lib.Configs;
using Abstract.Helpful.Lib.Logging;
using Abstract.Helpful.Lib.ProgramFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abstract.Helpful.AspNetCore
{
    public static class HostExtensions
    {
        private static ILogger logger;

        /// <summary>
        /// Standard set includes:
        /// * Startable <para/>
        /// * Disposable <para/>
        /// * TitleUpdater <para/>
        /// * ExceptionHandler
        /// </summary>
        public static IHost UseStandardSet<TLogger>(this IHost host, LogChannelBase criticalLogChannel, 
            ServiceName serviceName, OrganizationName organizationName = default, EnvironmentValue environment = default) 
            where TLogger : ILogger
        {
            var title = $"{serviceName} - " +
                        $"[Version:{Assembly.GetExecutingAssembly().GetName().Version}]";

            if (!organizationName.IsDefault())
                title = $"{organizationName} : {title}";
            
            var titleUpdater = new ConsoleTitleUpdater(title);

            ProgramExceptions.ServiceName = serviceName;
            ProgramExceptions.EnvironmentValue = environment;
            ProgramExceptions.Catch_Unhandled_And_Unobserved_Exceptions_WithLogger(() => logger, criticalLogChannel);
            ProgramExitHandler.Start();
            titleUpdater.Start();

            Task.WhenAll(host.Services.StartAllStartable()).GetAwaiter().GetResult();

            logger = host.Services.GetService<TLogger>();

            var disposables = host.Services.GetServices<IDisposable>();
            var disposablesAsync = host.Services.GetServices<IDisposableAsync>();

            ProgramExitHandler.SubscribeOnExit(() =>
            {
                titleUpdater.Dispose();
                ServiceProviderExtension.DisposeAll(logger, disposables, disposablesAsync).Wait();
            });

            return host;
        }
    }
}