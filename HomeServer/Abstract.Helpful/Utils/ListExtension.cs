using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib.Utils
{
    public static class ListExtension
    {
        [Safe]
        public static List<T> With<T>(this List<T> list, T elementToAdd)
        {
            list = list.EnsureNotNull();
            list.Add(elementToAdd);
            return list;
        }

        public static string ToPrettyString<TKey, TValue>(this Dictionary<TKey, TValue> dict, string separator = ", ")
        {
            return dict.Select(x => $"({x.Key}: {x.Value})").ToArray()
                .ToPrettyString(separator, false);
        }

        public static string ToPrettyString<T>(this IReadOnlyCollection<T> list, string separator = ", ", 
            bool shouldDisplayCount = true)
        {
            if (list == null)
                return "null";

            var builder = new StringBuilder();

            if (shouldDisplayCount)
                builder.Append($"(Count: {list.Count}){Environment.NewLine}");

            for (int i = 0; i < list.Count - 1; i++)
                builder.Append($"{list.ElementAt(i)}{separator}");

            if (list.Count != 0)
                builder.Append(list.Last());

            return builder.ToString();
        }

        public static string ToLowPrettyString<T>(this IReadOnlyCollection<T> list, char separator = '|')
        {
            if (list == null)
                return "null";

            var builder = new StringBuilder();

            foreach (var item in list)
                builder.Append($"{item}{separator}");

            return builder.ToString().TrimEnd(separator);
        }
        
        public static int MaxOrZero(this IReadOnlyCollection<int> ints)
        {
            if (ints.Count == 0)
                return 0;
            return ints.Max();
        }
        
        public static bool IsNullOrEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }
        
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
        
        public static bool IsNullOrEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
        
        public static async Task<List<T>> ToListAsync<T>(this Task<T[]> enumerableTask)
        {
            return (await enumerableTask).ToList();
        }

        public static async Task<List<T>> ToListAsync<T>(this Task<IEnumerable<T>> enumerableTask)
        {
            return (await enumerableTask).ToList();
        }

        public static async Task<List<T>> ToListAsync<T>(this Task<List<T>> listTask)
        {
            return (await listTask).ToList();
        }

    }
}