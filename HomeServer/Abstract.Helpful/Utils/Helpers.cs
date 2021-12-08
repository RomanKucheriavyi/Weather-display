using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace Abstract.Helpful.Lib.Utils
{
    public static class Helpers
    {
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list.Count == 0;
        }
        
        public static List<T> ToList<T>(this IEnumerable<T> enumerable, bool isSingleItemOnly)
        {
            if (isSingleItemOnly)
                return new List<T>
                {
                    enumerable.FirstOrDefault()
                };

            return enumerable.ToList();
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }


      

        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, string sortExpression)
        {
            var sortParts = sortExpression.Split(' ');
            var param = Expression.Parameter(typeof(T), string.Empty);
            try
            {
                var property = Expression.Property(param, sortParts[0]);
                var sortLambda =
                    Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), param);

                if (sortParts.Length > 1 && sortParts[1].Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    return source.AsQueryable().OrderByDescending(sortLambda);
                }
                return source.AsQueryable().OrderBy(sortLambda);
            }
            catch (ArgumentException)
            {
                return source;
            }
        }

        public static DataTable ToDataTable(this IEnumerable ien)
        {
            var dt = new DataTable();
            if (ien == null)
                return null;

            if (ien.GetType().IsGenericType)
            {
                var types = ien.GetType().GetGenericArguments();
                var pis = types[0].GetProperties();
                if (dt.Columns.Count == 0)
                    foreach (var pi in pis)
                        if (pi != null)
                            if (pi.PropertyType.IsGenericType)
                                if (IsNullableType(pi.PropertyType))
                                {
                                    var nc = new NullableConverter(pi.PropertyType);
                                    var underlyingType = nc.UnderlyingType;
                                    if (underlyingType != null)
                                        dt.Columns.Add(pi.Name, underlyingType);
                                }
                                else
                                    dt.Columns.Add(pi.Name);
                            else
                                dt.Columns.Add(pi.Name, pi.PropertyType);
            }

            foreach (var obj in ien)
            {
                if (obj == null) continue;
                var t = obj.GetType();
                if (t == null) continue;
                var pis = t.GetProperties();
                if (dt.Columns.Count == 0)
                    foreach (var pi in pis)
                        if (pi != null)
                            if (pi.PropertyType.IsGenericType)
                                dt.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType));
                            else
                                dt.Columns.Add(pi.Name, pi.PropertyType);

                var dr = dt.NewRow();
                foreach (var pi in pis)
                {
                    if (pi == null) continue;
                    var value = pi.GetValue(obj, null) ?? DBNull.Value;
                    dr[pi.Name] = value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static bool IsNullableType(this Type theType)
        {
            return (theType.IsGenericType && theType.
              GetGenericTypeDefinition().Equals
              (typeof(Nullable<>)));
        }

    }

}