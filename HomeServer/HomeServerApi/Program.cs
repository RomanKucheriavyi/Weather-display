using System.Threading.Tasks;
using Abstract.Helpful.Lib;
using Abstract.Helpful.Lib.Configs;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
// ReSharper disable MemberCanBePrivate.Global

namespace HomeServerApi
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class Program
    {
        private static string env;

        public static void Main(string[] args)
        {
            env = EnvironmentValue.Current();
            var host = CreateHostBuilder(args).Build();
            Task.WhenAll(host.Services.StartAllStartable()).GetAwaiter().GetResult();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(ConfigureKestrel);
                    webBuilder.UseStartup<Startup>();
                });
        
        private static void ConfigureKestrel(KestrelServerOptions serverOptions)
        {
            serverOptions.ListenLocalhost(6060, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            });
        }
    }
}