using System;

namespace Optsol.Components.Shared.Exceptions
{
    public class ConfigurationRepositoryException: Exception
    {
        public ConfigurationRepositoryException()
            : base($"As configurações do repositório estão incorretas.")
        {

        }
    }
}
