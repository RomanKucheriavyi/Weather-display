using System;
using System.Runtime.Serialization;

namespace Abstract.Helpful.Lib
{
    public sealed class TimeoutException : Exception
    {
        public TimeoutException()
        {
        }

        public TimeoutException(string message) : base(message)
        {
        }

        public TimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}