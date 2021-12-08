using Abstract.Helpful.Lib.Logging;

namespace Abstract.Helpful.Lib.Configs
{
    public sealed class LogEnvironmentParser : TypeParserBase<LogEnvironment>
    {
        protected override string ToRawString(LogEnvironment value)
        {
            return value.ToPrettyString();
        }

        protected override bool TryParse(string value, out LogEnvironment result)
        {
            result = value.ToLogEnvironment();
            return true;
        }
    }
}