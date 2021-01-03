using System;

namespace WebFramework.Exceptions
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message = null) : base(message)
        {
            
        }
    }
}