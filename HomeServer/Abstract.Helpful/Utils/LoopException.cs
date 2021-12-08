using System;
using System.Runtime.Serialization;

namespace Abstract.Helpful.Lib.Utils
{
    public sealed class LoopException : Exception
    {
        public LoopException()
        {
        }

        public LoopException(string message) : base(message)
        {
        }

        public LoopException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public LoopException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}