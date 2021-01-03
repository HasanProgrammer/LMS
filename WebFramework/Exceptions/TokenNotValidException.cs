using System;

namespace WebFramework.Exceptions
{
    public class TokenNotValidException : Exception
    {
        public TokenNotValidException(string message = null) : base(message)
        {
            
        }
    }
}