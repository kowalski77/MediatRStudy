using System;

namespace Publisher.App.Persistence
{
    public class UoWException : Exception
    {
        public UoWException()
        {
        }

        public UoWException(string message) : base(message)
        {
        }

        public UoWException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}