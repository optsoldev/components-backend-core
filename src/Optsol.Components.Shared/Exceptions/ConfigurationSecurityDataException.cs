using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class ConfigurationSecurityDataException<T> : Exception
    {
        public ConfigurationSecurityDataException()
            : base($"O {typeof(T).Name} resolvido pela injeção de dependência")
        {

        }
    }
}
