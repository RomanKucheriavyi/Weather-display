using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Abstract.Helpful.Lib
{
    public sealed class EnumStrings<TEnum>
    {
        public static readonly EnumStrings<TEnum> Instance = new();
        
        private readonly Dictionary<string, TEnum> _namesToValues = new(); 
        private readonly Dictionary<TEnum, string> _valuesToNames = new(); 
    
        public EnumStrings()
        {
            var enumType = typeof(TEnum);
            var enumValues = Enum.GetValues(enumType);
            foreach (var enumValue in enumValues)
            {
                var enumName = Enum.GetName(enumType, enumValue);
                var enumValueCasted = (TEnum) enumValue;
                _namesToValues.Add(enumName, enumValueCasted);
                _valuesToNames.Add(enumValueCasted, enumName);
            }
        }

        [Pure]
        [Safe]
        public TEnum Parse(string name)
        {
            if (_namesToValues.TryGetValue(name, out var value))
                return value;
            return default;
        }
        
        [Pure]
        [Safe]
        public bool TryParse(string name, out TEnum value)
        {
            return _namesToValues.TryGetValue(name, out value);
        }

        public string ToPrettyString(TEnum value)
        {
            if (_valuesToNames.TryGetValue(value, out var name))
                return name;
            return "Invalid";
        }
    }
}