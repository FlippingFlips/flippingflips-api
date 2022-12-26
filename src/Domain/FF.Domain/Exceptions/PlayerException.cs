using System;

namespace FF.Domain.Exceptions
{
    public class PlayerException : Exception
    {
        public PlayerException(string message) : base(message)
        {
        }
    }
}
