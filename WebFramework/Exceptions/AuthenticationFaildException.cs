using System;

namespace WebFramework.Exceptions
{
    public class AuthenticationFaildException : Exception
    {
        public AuthenticationFaildException(string message = null) : base(message)
        {
            
        }
    }
}