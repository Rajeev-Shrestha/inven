using System;

namespace HrevertCRM.Data.Common
{
    public class AuthorizationValidationException : Exception
    {
        public AuthorizationValidationException(string message)
            : this(message, null)
        {
        }

        public AuthorizationValidationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
