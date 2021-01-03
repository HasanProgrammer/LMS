using System;

namespace WebFramework.Exceptions
{
    public class TokenExpireException : Exception
    {
        public TokenExpireException(string message = null) : base(message)
        {
            
        }
    }
}