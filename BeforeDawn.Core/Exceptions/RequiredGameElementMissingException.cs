using System;

namespace BeforeDawn.Core.Exceptions
{
    class RequiredGameElementMissingException : Exception
    {
        public RequiredGameElementMissingException(string msg) : base(msg)
        {
            
        }
    }
}
