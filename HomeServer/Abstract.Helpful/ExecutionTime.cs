using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    public static class ExecutionTime
    {
        public static async Task<(T, TimeSpan)> Measure<T>(Func<Task<T>> func)
        {
            var s = new Stopwatch();
            s.Start();
            var t = await func();
            s.Stop();
            return (t, s.Elapsed);
        }
    }
}