using System;
using System.Collections.Generic;
using System.IO;
using Config.Net;
using GreenPipes.Internals.Extensions;
using Microsoft.Extensions.Configuration;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace Abstract.Helpful.Lib.Configs
{
    public static class ConfigsBuilderExtensions 
    {
        public static ConfigurationBuilder<T> UseAppSettings<T>(this ConfigurationBuilder<T> builder,
            EnvironmentKey environmentKey, string settingsDirectoryName = default)
            where T : class
        {
            var environment = Environment.GetEnvironmentVariable(environmentKey.Value,
                EnvironmentVariableTarget.Process);

            return UseAppSettings(builder, EnvironmentValue.From(environment), settingsDirectoryName);
        }
        
        public static ConfigurationBuilder<T> UseAppSettings<T>(this ConfigurationBuilder<T> builder, 
            EnvironmentValue environmentValue = default, string settingsDirectoryName = default)
            where T : class
        {
            IConfigurationRoot Read(string file)
            {
                if (!settingsDirectoryName.IsNullOrEmpty())
                    file = Path.Combine(settingsDirectoryName, file);
                
                return new ConfigurationBuilder()
                    .AddJsonFile(file)
                    .Build();
            }

            var defaultConfiguration = Read("appsettings.json");
            var environment = environmentValue.Value;

            var isEnvironmentSpecified = !string.IsNullOrEmpty(environment);
            if (isEnvironmentSpecified)
            {
                var configuration = Read($"appsettings.{environment}.json");
                builder.UseAppSettings(configuration);
            }

            builder.UseAppSettings(defaultConfiguration);

            return builder;
        }
        public static void UseAppSettings<TConfigs>(this ConfigurationBuilder<TConfigs> builder,
            IConfiguration configuration) where TConfigs : class
        {
            var appSettingsConfigs = GetAppSettingsConfigs<TConfigs>(configuration);
            builder.UseInMemoryDictionary(appSettingsConfigs);
        }

        private static Dictionary<string, string> GetAppSettingsConfigs<TConfigs>(IConfiguration configuration)
        {
            var inMemoryConfigs = new Dictionary<string, string>();
            if (configuration == null) return inMemoryConfigs;

            try
            {
                var section = configuration.GetSection(ConfigsReader<object>.ConfigsSectionName);
                
                if (!section.Exists())
                    return inMemoryConfigs;

                foreach (var propertyInfo in typeof(TConfigs).GetAllProperties())
                {
                    var key = propertyInfo.Name;
                    var value = section.GetValue<string>(key);
                    
                    if (value == null)
                        continue;
                    
                    inMemoryConfigs.Add(key, value);
                }
            }
            catch (Exception)
            {
                //ignore
            }
            return inMemoryConfigs;
        }

        public static void VerifyConfig<T>(this T config)
        {
            foreach (var propertyInfo in typeof(T).GetAllProperties())
            {
                var property = propertyInfo.GetValue(config);
                if (property == null)
                    throw new Exception($"Property {propertyInfo.Name} is null");
            }
        }
    }
}