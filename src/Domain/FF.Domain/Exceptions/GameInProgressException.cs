using System;

namespace FF.Domain.Exceptions
{
    public class GameInProgressException : Exception
    {
        public GameInProgressException(string message) : base(message)
        {
        }
    }
}
