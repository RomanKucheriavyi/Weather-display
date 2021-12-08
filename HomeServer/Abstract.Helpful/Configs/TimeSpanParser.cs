using System;
using System.Globalization;

namespace Abstract.Helpful.Lib.Configs
{
    public sealed class TimeSpanParser : TypeParserBase<TimeSpan>
    {
        protected override string ToRawString(TimeSpan value)
        {
            return value.TotalSeconds.ToString(CultureInfo.InvariantCulture);
        }

        protected override bool TryParse(string value, out TimeSpan result)
        {
            if (!int.TryParse(value, out var seconds))
            {
                result = default;
                return false;
            }

            result = TimeSpan.FromSeconds(seconds);
            return true;
        }
    }
}