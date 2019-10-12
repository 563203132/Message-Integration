using System;

namespace Message.Integration.Common.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException() : base() { }

        public MessageException(int code, string message) : base(message)
        {
            Code = code;
        }

        public MessageException(int code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }

        public int Code { get; set; }
    }
}
