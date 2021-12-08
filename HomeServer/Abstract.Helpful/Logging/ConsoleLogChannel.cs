namespace Abstract.Helpful.Lib.Logging
{
    public sealed class ConsoleLogChannel : LogChannelBase
    {
        public static readonly ConsoleLogChannel Instance = new();
        
        public override LogChannelType Type { get; } = LogChannelType.Console;

        private ConsoleLogChannel()
        {
        }
    }
}