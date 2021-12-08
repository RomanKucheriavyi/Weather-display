namespace Abstract.Helpful.Lib.Logging
{
    public sealed class TelegramLogChannel : LogChannelBase
    {
        public override LogChannelType Type { get; } = LogChannelType.Telegram;
        public string TelegramBotToken { get; }

        public TelegramLogChannel(string telegramBotToken)
        {
            TelegramBotToken = telegramBotToken;
        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(TelegramBotToken)}: {TelegramBotToken}";
        }
    }
}