using System;
using System.Diagnostics.Contracts;

namespace Abstract.Helpful.Lib.Utils
{
    public static class DateTimeService
    {
        private static DateTime substitutedNow = default;
        private static bool isNowSubstituted = false;

        public static DateTime UtcNow
        {
            get
            {
                if (isNowSubstituted)
                    return substitutedNow;
                    
                return DateTime.Now.ToUniversalTime();
            }
        }

        public static DateTime NewUTC(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
        {
            return new(year, month, day, hour, minute, second, DateTimeKind.Utc);
        }
        
        [Pure]
        [Safe]
        public static DateTime ToUTC(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime();
        }

        [Pure]
        [Safe]
        public static DateTime ToDayTime(this DateTime dateTime)
        {
            return NewUTC(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        public static void SubstituteNow(DateTime now)
        {
            isNowSubstituted = true;
            substitutedNow = now;
        }

        public static void CancelSubstitute()
        {
            isNowSubstituted = false;
        }

        public static IDisposable FreezeNow()
        {
            return UseNowSubstitution(DateTime.Now);
        }

        public static IDisposable UseNowSubstitution(DateTime now)
        {
            SubstituteNow(now);
            return new DisposableAction(CancelSubstitute);
        }

        public static void WriteLineDebug(string text = "") => UtcNow.WriteLineDebug(text);

        public static DateTime ToDateTime(long unix)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var date= start.AddMilliseconds(unix).ToUniversalTime();
            return date;
        }

        [Pure]
        public static bool IsExpired(this DateTime dateTime, TimeSpan expirationPeriod)
        {
            return  dateTime.Add(expirationPeriod) < UtcNow;
        }
    }
}