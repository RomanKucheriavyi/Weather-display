using System;

namespace Abstract.Helpful.Lib.Logging
{
    [Flags]
    public enum LogEnvironment
    {
        Production = 1,
        Development = 1 << 1,
        None = 1 << 2,
        Any = Production | Development,
    }


    public static class LogEnvironmentExtension
    {
        private static readonly EnumStrings<LogEnvironment> strings = new();
    
        public static LogEnvironment ToLogEnvironment(this string logEnvironment)
        {
            return strings.Parse(logEnvironment);
        }

        public static string ToPrettyString(this LogEnvironment logEnvironment)
        {
            return strings.ToPrettyString(logEnvironment);
        }
    }
}