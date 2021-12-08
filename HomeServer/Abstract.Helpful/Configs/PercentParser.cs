using Abstract.Helpful.Lib.Utils;

namespace Abstract.Helpful.Lib.Configs
{
    public sealed class PercentParser : TypeParserBase<Percent>
    {
        protected override string ToRawString(Percent value)
        {
            return value.ToOneHundredFormatInt().ToString();
        }

        protected override bool TryParse(string value, out Percent result)
        {
            if (!int.TryParse(value, out var percents))
            {
                result = default;
                return false;
            }

            result = Percent.FromOneHundredFormat(percents);
            return true;
        }
    }
}