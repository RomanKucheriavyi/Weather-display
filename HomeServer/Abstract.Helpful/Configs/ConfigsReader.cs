using System;
using System.Collections.Generic;
using System.Linq;
using Config.Net;
using Microsoft.Extensions.Configuration;

namespace Abstract.Helpful.Lib.Configs
{
    public class ConfigsReader<TConfigs> where TConfigs : class
    {
        public static string ConfigsSectionName = "IAppConfigs";
        public string EnvironmentKey { get; set; } = "ASPNETCORE_ENVIRONMENT";
        public Action<ConfigurationBuilder<TConfigs>> Configure { get; set; } = null;
        public EnvironmentValue EnvironmentValue { get; set; } = null;
        public Dictionary<string, string> CustomValues { get; set; } = null;
        public bool IsForcedToNotUseConfiguration { get; set; } = false;
        public string SettingsDirectoryName { get; set; } = null;

        public TConfigs ReadConfigs(IConfiguration configuration = null, string[] args = null)
        {
            if (CustomValues != null)
                args = ConfigsReader.PrepareArgs(CustomValues, args);
            
            var builder = new ConfigurationBuilder<TConfigs>();
        
            builder.UseCommandLineArgs(false, args);

            if (configuration == null || IsForcedToNotUseConfiguration || !configuration.GetSection(ConfigsSectionName).Exists())
            {
                if (EnvironmentValue.Value.IsNullOrEmpty())
                    builder.UseAppSettings(Configs.EnvironmentKey.From(EnvironmentKey), SettingsDirectoryName);
                else
                    builder.UseAppSettings(Configs.EnvironmentValue.From(EnvironmentValue), SettingsDirectoryName);
            }
            else
                builder.UseAppSettings(configuration);
        
            builder.UseTypeParser(new PercentParser());
            builder.UseTypeParser(new TimeSpanParser());
            builder.UseTypeParser(new StringArrayParser());
            builder.UseTypeParser(new LogEnvironmentParser());

            Configure?.Invoke(builder);
       
            var configs = builder.Build();

            configs.VerifyConfig();

            return configs;
        }
    }

    public static class ConfigsReader
    {
        public static string[] PrepareArgs(Dictionary<string, string> customValues, string[] args = null)
        {
            var argsList = args == null ? new List<string>() : args.ToList();

            void Configs_Args_SetField(string fieldName, string fieldValue)
            {
                int index = -1;
                try
                {
                    index = argsList.FindLastIndex(s => s.Contains(fieldName));
                }
                catch
                {
                    index = -1;
                }

                var value = $"{fieldName}={fieldValue}";

                if (index != -1)
                    argsList[index] = value;
                else
                    argsList.Add(value);
            }

            foreach (var customValue in customValues)
            {
                Configs_Args_SetField(customValue.Key, customValue.Value);
            }

            return argsList.ToArray();
        }
    }
}