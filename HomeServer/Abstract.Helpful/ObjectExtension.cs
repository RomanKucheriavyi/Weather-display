using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace Abstract.Helpful.Lib
{
    /// <summary>
    /// instance.IsDefault() can return true
    /// </summary>
    public sealed class CanBeDefaultAttribute : Attribute
    {
        /// <summary>
        /// instance.IsDefault() can return true
        /// </summary>
        public CanBeDefaultAttribute()
        {
        }
    }
    
    /// <summary>
    /// Method designed with rule: SHOULD NOT THROW EXCEPTIONS!
    /// </summary>
    public sealed class SafeAttribute : Attribute
    {
        /// <summary>
        /// Method designed with rule: SHOULD NOT THROW EXCEPTIONS!
        /// </summary>
        public SafeAttribute()
        {
        }
    }
    
    public sealed class DeprecatedAttribute : Attribute
    {
        public DeprecatedAttribute()
        {
        }
    }

    public interface IDefaultable
    {
        bool IsDefault { get; }
    }

    public static class EnumExtensions
    {
        [Pure]
        [Safe]
        public static string EnumToPrettyString<TEnum>(this TEnum @enum)
        {
            return EnumStrings<TEnum>.Instance.ToPrettyString(@enum);
        }
        
        [Pure]
        [Safe]
        public static TEnum ParseEnum<TEnum>(this string @string)
        {
            return EnumStrings<TEnum>.Instance.Parse(@string);
        }
        
        [Pure]
        [Safe]
        public static bool TryParseEnum<TEnum>(this string @string, out TEnum value)
        {
            return EnumStrings<TEnum>.Instance.TryParse(@string, out value);
        }
    }
    
    public static class RandomString
    {
        private static readonly Random random = new();
        private const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string Next()
        {
            return new(Enumerable.Repeat(CHARS, random.Next(10, 20))
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public static class ObjectExtension
    {
        [Pure]
        [Safe]
        public static string Capitalize(this string input)
        {
            if (input.IsNullOrEmpty())
                return input;
            
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
        
        public static T Second<T>(this IEnumerable<T> enumerable) => enumerable.ElementAt(1);
        public static T Third<T>(this IEnumerable<T> enumerable) => enumerable.ElementAt(2);

        [Pure]
        [Safe]
        public static TValue GetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> createFunc)
        {
            if (dictionary.TryGetValue(key, out var value))
                return value;

            var newValue = createFunc();
            dictionary.Add(key, newValue);
            return newValue;
        }
        
        public static async Task<Dictionary<TKey, TValue>> ToDictionary<TKey, TValue>(this Task<List<TValue>> valuesTask, 
            Func<TValue, TKey> keySelector)
        {
            var values = await valuesTask;
            return values?.ToDictionary(keySelector) ?? new Dictionary<TKey, TValue>();
        }
        
        public static Guid NewIfDefault(this Guid guid)
        {
            if (guid.IsDefault())
                return Guid.NewGuid();
            return guid;
        }
    
        public static string ToStringSafe<T>(this T t) where T : class
        {
            if (t == null)
                return "null";
            return t.ToString();
        }

        public static List<T> With<T>(this List<T> list, IEnumerable<T> enumerable)
        {
            list.AddRange(enumerable);
            return list;
        }
    
        public static int ToInt(this uint n) => (int) n;
        
        public static IEnumerable<T> ConditionalWhere<T>(this IEnumerable<T> enumerable,  bool @if,
            Func<T, bool> trueCase,
            Func<T, bool> falseCase = default)
        {
            if (@if)
                return enumerable.Where(trueCase);
            
            if (falseCase == default)
                return enumerable;

            return enumerable.Where(falseCase);
        }

        public static void Rethrow(this List<Exception> exceptions)
        {
            if (exceptions.Count == 1)
                exceptions.First().Rethrow();
            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);
        }
        
        public static void Rethrow(this Exception exception)
        {
            // https://blog.adamfurmanek.pl/2016/10/01/handling-and-rethrowing-exceptions-in-c/
            ExceptionDispatchInfo.Capture(exception).Throw();
        }

        public static void ApplyPropertyLimit<T>(this IEnumerable<T> entities, int limitMax, int limitActual, 
            Action<T> disableProperty, Action<T> enableProperty)
        {
            entities
                .ForEach(disableProperty)
                .Take(limitMax - limitActual)
                .ForEach(enableProperty);
        }
        
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var arr = enumerable.ToArray();
            foreach (var e in arr)
                action(e);
            return arr;
        }

        
        public static T SyncWait<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
        
        public static void SyncWait(this Task task)
        {
            task.GetAwaiter().GetResult();
        }
        
        public static string WithoutException(this string str)
        {
            return str.Replace(nameof(Exception), string.Empty);
        }
        
        public static T ReplaceIfDefault<T>(this T t, Func<T> replaceFunc)
        {
            return t.IsDefault() ? replaceFunc() : t;
        }
        
        public static T ReplaceIfDefault<T>(this T t, T replace)
        {
            return t.IsDefault() ? replace : t;
        }
        
        public static void RemoveMany<T>(this List<T> list, IEnumerable<T> toRemove)
        {
            foreach (var t in toRemove)
            {
                list.Remove(t);
            }
        }
        
        public static void AddMany<T>(this HashSet<T> hashSet, IEnumerable<T> toAdd)
        {
            foreach (var t in toAdd)
            {
                hashSet.Add(t);
            }
        }
        
        public static Dictionary<TKey, TValue> RemoveDefaultEntries<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var keysToDelete = dictionary
                .Where(x => x.Value.IsDefault())
                .Select(x => x.Key)
                .ToArray();

            foreach (var key in keysToDelete)
                dictionary.Remove(key);
            
            return dictionary;
        }
        
        public static List<T> EnsureNotNull<T>(this List<T> list, int capacity = default)
        {
            if (list == null)
                return capacity.IsDefault() ? new List<T>() : new List<T>(capacity);
            return list;
        }
        
        public static T[] EnsureNotNullArray<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return new T[0];
            return enumerable.ToArray();
        }
        
        public static List<T> EnsureNotNullList<T>(this List<T> enumerable)
        {
            if (enumerable == null)
                return new List<T>();
            return enumerable;
        }
        
        public static Dictionary<TKey, TValue> EnsureNotNullDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                return new Dictionary<TKey, TValue>();
            return dictionary;
        }
        
        public static List<T> EnsureNotNullList<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return new List<T>();
            return enumerable.ToList();
        }
        
        public static IEnumerable<T> EnsureNotNullEnumerable<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                return new T[0];
            return enumerable;
        }
        
        public static bool AddUnique<T>(this List<T> list, IEnumerable<T> addValues)
        {
            var result = false;
            foreach (var addValue in addValues)
                result |= list.AddUnique(addValue);
            return result;
        }
        
        public static bool AddUnique<T>(this List<T> list, T addValue)
        {
            if (list.Contains(addValue))
                return false;
            
            list.Add(addValue);
            return true;
        }
        
        public static T GetOrAdd<T>(this List<T> list, Func<T, bool> predicate, Func<T> newFunc)
        {
            var elem = list.FirstOrDefault(predicate);
            if (elem.IsDefault())
            {
                elem = newFunc();
                list.Add(elem);
            }
            return elem;
        }
        
        public static bool TryRemove<T>(this List<T> list, T element)
        {
            if (list == null)
                return false;

            return list.Remove(element);
        }
            
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, out T found)
        {
            found = enumerable.FirstOrDefault(predicate);
            return !found.IsDefault();
        }
        
        public static bool SequenceEqualAdvanced<T>(this List<T> list, List<T> other)
        {
            if (list == null)
                return other == null;

            if (other == null)
                return false;

            return list.SequenceEqual(other);
        }
    
        [Pure]
        public static string ToBase64Utf8(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var base64 = Convert.ToBase64String(bytes);
            return base64;
        }
        
        [Pure]
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            var dict = new Dictionary<TKey, TValue>();
            foreach (var pair in dictionary)
                dict.Add(pair.Key, pair.Value);
            return dict;
        }

        [Pure]
        public static TValue GetOrDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            if (dictionary == null)
                return defaultValue;
            if (dictionary.TryGetValue(key, out var value))
                return value;
            return defaultValue;
        }
        
        [Pure]
        public static bool IsNullOrEmpty(this string param)
        {
            return string.IsNullOrEmpty(param?.Trim());
        }
      
        [Pure]
        public static SortedList<string, int> CountWordOccurrences(this string sourceText)
        {
            var words = sourceText.Split(' ');
            var wordList = new SortedList<string, int>();
            foreach (var word in words)
            {
                if (!(wordList.ContainsKey(word)))
                {
                    wordList.Add(word, 1);
                }
                else
                {
                    var iWordCount = wordList[word];
                    wordList[word] = iWordCount + 1;
                }
            }
            return wordList;
        }

      

        [Pure]
        public static string Remove(this string text, string remove)
        {
            return text.Replace(remove, string.Empty);
        }
        
        public static int DivideOrZero(this int divideLeft, int divideRight)
        {
            return divideLeft.DivideOrValue(divideRight, 0);
        }
        
        public static int DivideOrValue(this int divideLeft, int divideRight, int value)
        {
            return divideRight != 0 ? divideLeft / divideRight : value;
        }
        
        public static int ToIntSafe(this long @long)
        {
            return @long > int.MaxValue
                ? int.MaxValue
                : (int) @long;
        }
        
        [Pure]
        [Safe]
        public static uint ToUintSafe(this long @long)
        {
            if (@long > uint.MaxValue)
                return uint.MaxValue;

            if (@long < 0)
                return 0;

            return (uint) @long;
        }

        public static List<T> SelectRandomElements<T>(this List<T> list, uint count)
        {
            if (list.Count <= count)
                return list;
            
            var selectedList = new List<T>();
            var listCopy = list.ToList();
            var random = new Random();
            while (selectedList.Count != count && listCopy.Count > 0)
            {
                var selectedElementIndex = random.Next(0, listCopy.Count);
                var selectedElement = listCopy[selectedElementIndex];
                selectedList.Add(selectedElement);
                listCopy.RemoveAt(selectedElementIndex);
            }
            return selectedList;
        }
        
        [Safe]
        [Pure]
        public static uint ToUint(this int @int)
        {
            if (@int < 0)
                return 0;
            return (uint) @int;
        }
        
        public static void WriteLineDebug(this DateTime dateTime, string text = "")
        {
            Console.WriteLine($"----------------- {dateTime:O} ---- {text} ----- ");
        }
    
        public static ConfiguredTaskAwaitable ConfigureAwaitFalse(this Task task)
        {
            return task.ConfigureAwait(false);
        }
        
        public static ConfiguredTaskAwaitable<T> ConfigureAwaitFalse<T>(this Task<T> task)
        {
            return task.ConfigureAwait(false);
        }
    
        public static bool IsLaterThen(this DateTime laterTime, DateTime earlierTime)
        {
            return laterTime.CompareTo(earlierTime) > 0;
        }
        
        public static bool IsEarlierThen(this DateTime earlierTime, DateTime laterTime)
        {
            return laterTime.IsLaterThen(earlierTime);
        }
        
        public static bool IsDefault<T>(this T obj) 
        {
            return obj == null || obj.Equals(default(T));
        }
        
        public static bool IsDefault(this IDefaultable obj) 
        {
            return obj == null || obj.IsDefault;
        }

        public static bool TryCastTo<T>(this object obj, out T casted)
        {
            if (obj is T castedObj)
            {
                casted = castedObj;
                return true;
            }

            casted = default(T);
            return false;
        }

        /// <summary>
        ///     WARNING: not thread safe
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, 
            Func<TValue> add,
            Func<TValue, TValue> update)
        {
            if (dictionary.ContainsKey(key))
                dictionary[key] = update(dictionary[key]);
            else
                dictionary.Add(key, add());
        }
        
        /// <summary>
        ///     WARNING: not thread safe
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, 
            TValue addValue, Func<TValue, TValue> update)
        {
            dictionary.AddOrUpdate(key, () => addValue, update);
        }
        
        /// <summary>
        ///     WARNING: not thread safe
        /// </summary>
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, 
            TValue addValue)
        {
            dictionary.AddOrUpdate(key, () => addValue, (_) => addValue);
        }

        /// <summary>
        ///     WARNING: not thread safe
        /// </summary>
        public static void Remove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> removeCriteria)
        {
            var keys = dictionary.Keys.ToArray();
            foreach (var key in keys)
            {
                if (removeCriteria(key, dictionary[key]))
                    dictionary.Remove(key);
            }
        }
        
        [Pure]
        public static bool Exist(this object obj)
        {
            return obj != null;
        }

        public static int RoundToInt(this double @double)
        {
            return (int) Math.Round(@double);
        }
        
        public static int RoundToInt(this float @float)
        {
            return (int) Math.Round(@float);
        }
    }

    public static class BoolsCounter
    {
        public static uint Count(params bool[] bools)
        {
            if (bools.IsNullOrEmpty())
                return 0;

            // ReSharper disable once RedundantBoolCompare
            return (uint) bools.Count(@bool => @bool == true);
        }
    }
}