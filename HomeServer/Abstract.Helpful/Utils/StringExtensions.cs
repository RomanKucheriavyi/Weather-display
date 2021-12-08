using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Abstract.Helpful.Lib.Utils
{
    public static class StringExtensions
    {
        [Pure]
        public static string[] Split(this string value, string pattern)
        {
            return value?.Split(new[] {pattern}, StringSplitOptions.None)
                       .Where(d => d.Trim().Length > 0).ToArray() ??
                   new string[0];
        }


        [Pure]
        public static string[] GetWords(this string value)
        {
            return ObjectExtension.IsNullOrEmpty(value) ? new string[] { } : value.Split(@" ");
        }
        
        
        /// <summary>
        ///     Determines whether the comparison value strig is contained within the input value string
        /// </summary>
        /// <param name="inputValue"> The input value. </param>
        /// <param name="comparisonValue"> The comparison value. </param>
        /// <param name="comparisonType"> Type of the comparison to allow case sensitive or insensitive comparison. </param>
        /// <returns> <c>true</c> if input value contains the specified value, otherwise, <c>false</c> . </returns>
        public static bool Contains(this string inputValue, string comparisonValue, StringComparison comparisonType)
        {
            return (inputValue.IndexOf(comparisonValue, comparisonType) != -1);
        }
        
        
        
        [Pure]
        public static string StripHtml(string html)
        {
            return string.IsNullOrEmpty(html) 
                ? string.Empty 
                : Regex.Replace(html, @"<(.|\n)*?>", string.Empty);
        }
        
        
        public static string TextTruncate(this string result, int maxLength)
        {
            if (string.IsNullOrEmpty(result))
                return string.Empty;

            if (result.Length > maxLength)
                result = $"{result.Substring(0, maxLength - 1)}...";

            return result;
        }
        
        public static string Serialize(this object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, settings);
        }

        public static T SerializeRevert<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }
        
        [Pure]
        public static bool IsNullOrEmptyOrZero(this string param)
        {
            if (string.IsNullOrWhiteSpace(param))
                return true;

            return param == "0";
        }
        
        [Pure]
        public static bool IsNullOrWhiteSpace(this string param)
        {
            return string.IsNullOrWhiteSpace(param);
        }
        
        
        /// <summary>
        ///     Determines whether the specified string is null or empty.
        /// </summary>
        /// <param name="value"> The string value to check. </param>
        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     Determines whether the specified string is not null or empty.
        /// </summary>
        /// <param name="value"> The string value to check. </param>
        public static bool IsNotEmpty(this string value)
        {
            return (value.IsEmpty() == false);
        }

     

        public static bool Contains(this string inputValue, string comparisonValue, bool ignoreCase = true)
        {
            if (inputValue == null)
                return false;
            return
                (inputValue.IndexOf(comparisonValue,
                     ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.Ordinal) !=
                 -1);
        }
        
 
        
       
        
        public static string ToDashedLowerCase(this string text) 
        {
            return string
                .Concat(text.Select((x, i) =>
                {
                    return i > 0 && char.IsUpper(x) 
                        ? '-' + x.ToString()
                        : x.ToString();
                }))
                .ToLower();
        }
        
       
        
    }
}