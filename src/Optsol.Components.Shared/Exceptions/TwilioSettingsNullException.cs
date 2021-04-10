using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class TwilioSettingsNullException : Exception
    {
        public TwilioSettingsNullException()
            : base("O parametro TwilioSettings não foi resolvido pela injeção de dependência")
        {

        }
    }
}
