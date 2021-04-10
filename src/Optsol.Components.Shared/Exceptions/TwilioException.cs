using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class TwilioException : Exception
    {
        public TwilioException(string message)
            : base(message)
        {
        }
    }
}
