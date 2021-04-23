using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public sealed class ConfigurationRepositoryException: Exception
    {
        public ConfigurationRepositoryException()
            : base($"As configurações do repositório estão incorretas.")
        {

        }
    }
}
