using System.Threading.Tasks;

namespace Abstract.Helpful.Lib.Logging
{
    public interface ILogger
    {
        Task LogAsync(
            LogText logText, 
            LogChannelBase logChannel,
            LogEnvironment logEnvironment = LogEnvironment.Any,
            LogType logType = LogType.Information);
    }
}