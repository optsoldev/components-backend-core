using System;
using System.Runtime.Serialization;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class ConfigurationRepositoryException: Exception
    {
        public ConfigurationRepositoryException()
            : base("As configurações do repositório estão incorretas.")
        {

        }

        protected ConfigurationRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
