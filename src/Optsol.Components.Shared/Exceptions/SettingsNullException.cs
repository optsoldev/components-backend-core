using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    public class SettingsNullException : Exception
    {
        public SettingsNullException(string objectName)
            : base($"O Atributo {objectName} está nulo")
        {

        }

        protected SettingsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
