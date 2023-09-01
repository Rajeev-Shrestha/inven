using System;

namespace HrevertCRM.Data.Common
{
    public class VersionMismatchException : Exception
    {
        public VersionMismatchException(string message) : base(message)
        {
            
        }
    }
}
