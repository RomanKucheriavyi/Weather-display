using System;
using System.Collections.Generic;
using System.Linq;
using Config.Net;

namespace Abstract.Helpful.Lib.Configs
{
    public sealed class StringArrayParser : ITypeParser
    {
        public bool TryParse(string value, Type t, out object result)
        {
            if (t != typeof(string[]))
            {
                result = null;
                return false;
            }

            if (value == string.Empty)
            {
                result = new string[0];
            }
            else
            {
                result = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToArray();
            }

            return true;
        }

        public string ToRawString(object value)
        {
            var strings = (string[]) value;
            return strings.Aggregate((a, b) => a + "," + b);
        }

        public IEnumerable<Type> SupportedTypes { get; } = new List<Type>{typeof(string[])};
    }
}