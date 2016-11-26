using System;

namespace ModsDeApi.Services.Thread
{
    public class InvalidThreadIdException : Exception
    {
        public int ThreadId { get; }

        public InvalidThreadIdException(int threadId) : base()
        {
            ThreadId = threadId;
        }

        public InvalidThreadIdException(int threadId, string message) : base(message)
        {
            ThreadId = threadId;
        }

        public InvalidThreadIdException(int threadId, string message, Exception innerException) : base(message, innerException)
        {
            ThreadId = threadId;
        }
    }
}
