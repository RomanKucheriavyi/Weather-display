using System;
using System.Diagnostics.Contracts;

namespace Abstract.Helpful.Lib
{
    /// <summary>
    ///     Extension methods for the DateTimeOffset data type.
    /// </summary>
    public static class DateTimeExtensions
    {
        [Pure]
        public static int CalculateDaysPassed(this DateTime dateNow)
        {
            return CalculateDaysPassed((DateTime?) dateNow);
        }

        /// <summary>
        ///     Calculates the age in days based on today.
        /// </summary>
        /// <param name="dateNow"> The date of birth. </param>
        /// <returns> The calculated age. </returns>
        [Pure]
        public static int CalculateDaysPassed(this DateTime? dateNow)
        {
            if (dateNow == null)
                return 0;

            var dt1 = DateTime.Now;
            var dt2 = dateNow;
            var ts = dt1.Subtract(dt2.Value);
            return ts.Days;
        }
    }
}