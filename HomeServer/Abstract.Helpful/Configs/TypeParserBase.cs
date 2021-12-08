using System;
using System.Collections.Generic;
using Config.Net;

namespace Abstract.Helpful.Lib.Configs
{
    public abstract class TypeParserBase<T> : ITypeParser
    {
        public bool TryParse(string value, Type t, out object result)
        {
            if (t != typeof(T))
            {
                result = null;
                return false;
            }

            var isParsed = TryParse(value, out var resultT);
            result = resultT;
            return isParsed;
        }

        public string ToRawString(object value)
        {
            return ToRawString((T) value);
        }

        protected abstract string ToRawString(T value);
        protected abstract bool TryParse(string value, out T result);

        public IEnumerable<Type> SupportedTypes { get; } = new List<Type>
        {
            typeof(T)
        };
    }
}