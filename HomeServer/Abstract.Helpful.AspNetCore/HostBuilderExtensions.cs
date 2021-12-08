using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abstract.Helpful.AspNetCore
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseTestEnvironment(this IHostBuilder hostBuilder, string key = "ASPNETCORE_ENVIRONMENT")
        {
            var aspnetcoreEnvironmentVariable = Environment
                .GetEnvironmentVariable(key, EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(aspnetcoreEnvironmentVariable))
                aspnetcoreEnvironmentVariable = string.Empty;

            var value = aspnetcoreEnvironmentVariable.Contains("Test") 
                ? aspnetcoreEnvironmentVariable 
                : aspnetcoreEnvironmentVariable + "Test";
            
            hostBuilder.UseEnvironment(value);

            return hostBuilder;
        }

        public static IHostBuilder UseAutofac(this IHostBuilder builder)
        {
            return builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        }
    }
}