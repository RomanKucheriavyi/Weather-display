namespace Abstract.Helpful.Lib.Logging
{
    public sealed class StorageLogChannel : LogChannelBase
    {
        public static readonly StorageLogChannel Instance = new();
        
        public override LogChannelType Type { get; } = LogChannelType.Storage;

        private StorageLogChannel()
        {
        }
    }
}