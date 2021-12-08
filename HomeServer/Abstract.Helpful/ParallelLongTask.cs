using System;
using System.Threading.Tasks;

namespace Abstract.Helpful.Lib
{
    /// <summary>
    /// Task.Factory.StartNew(func, TaskCreationOptions.LongRunning);
    /// </summary>
    public static class ParallelLongTask
    {
        /// <summary>
        /// Task.Factory.StartNew(func, TaskCreationOptions.LongRunning);
        /// </summary>
        public static Task Start(Func<Task> func)
        {
            return Task.Factory.StartNew(func, TaskCreationOptions.LongRunning);
        }
    }
}