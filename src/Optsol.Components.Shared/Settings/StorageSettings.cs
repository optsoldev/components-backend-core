using System;

namespace Optsol.Components.Shared.Settings
{
    public class StorageSettings : BaseSettings
    {
        public Blob Blob { get; set; }

        public string ConnectionString { get; set; }

        public override void Validate()
        {
            var connectionIsNull = string.IsNullOrEmpty(ConnectionString);
            if (connectionIsNull)
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }
        }
    }

    public class Blob : BaseSettings
    {
        public string ContainerName { get; set; }

        public override void Validate()
        {
            var containerNameIsNull = string.IsNullOrEmpty(ContainerName);
            if (containerNameIsNull)
            {
                throw new ArgumentNullException(nameof(ContainerName));
            }
        }
    }
}
