using System;

namespace FF.Domain.Exceptions
{
    public class NoGameFoundException : Exception
    {
        public NoGameFoundException(string? message = null) : base(message)
        {
            if(message == null)
                message = "No game was found!";
        }
    }
}
