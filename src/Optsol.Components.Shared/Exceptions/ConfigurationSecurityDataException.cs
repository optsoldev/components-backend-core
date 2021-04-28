using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class ConfigurationSecurityDataException<T> : Exception
    {
        public ConfigurationSecurityDataException()
            : base($"O {typeof(T).Name} resolvido pela injeção de dependência")
        {

        }

        protected ConfigurationSecurityDataException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
