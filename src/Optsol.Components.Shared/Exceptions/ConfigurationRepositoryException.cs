using System;

namespace Optsol.Components.Shared.Exceptions
{
    [Serializable]
    public class ConfigurationRepositoryException: Exception
    {
        public ConfigurationRepositoryException()
            : base($"As configurações do repositório estão incorretas.")
        {

        }
    }
}
