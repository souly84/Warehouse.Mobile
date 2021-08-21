using System;

namespace Warehouse.Mobile.Plugins
{
    public class UnknownExceptionTypeException : Exception
    {
        public UnknownExceptionTypeException(string message, string notFoundException)
            : base(message)
        {
            NotFoundException = notFoundException;
        }

        public UnknownExceptionTypeException(
           string message,
           string notFoundException,
           Exception innerException)
           : base(message, innerException)
        {
            NotFoundException = notFoundException;
        }

        public string NotFoundException { get; }
    }
}
