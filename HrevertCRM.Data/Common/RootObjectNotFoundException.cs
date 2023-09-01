using System;

namespace HrevertCRM.Data.Common
{
    public class RootObjectNotFoundException : Exception
    {
        public RootObjectNotFoundException( string message) : base(message)
        {
            
        }
    }
}
