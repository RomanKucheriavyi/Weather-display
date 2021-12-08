using System;
using System.Collections.Generic;

namespace Abstract.Helpful.Lib.Logging
{
    [Flags]
    public enum LogType
    {
        Information = 1,
        Warning = 1 << 1,
        Error = 1 << 2,
        Success = 1 << 3,
        All = Information | Warning | Error | Success,
    }

    public static class LogTypeExtension
    {
        private static readonly Dictionary<LogType, string> _strings = new()
        {
            {LogType.Information, "Information"},
            {LogType.Warning, "Warning"},
            {LogType.Error, "Error"},
            {LogType.Success, "Success"},
            {LogType.All, "All"},
        };
        
        public static string ToPrettyString(this LogType logType)
        {
            try
            {            
                return _strings[logType];
            }
            catch
            {
                return "Mix";
            }
        }
    }
}