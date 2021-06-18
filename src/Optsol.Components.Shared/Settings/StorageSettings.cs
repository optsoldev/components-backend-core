using System;

namespace Optsol.Components.Shared.Settings
{
    public class StorageSettings : BaseSettings
    {
        public string ConnectionString { get; set; }

        public override void Validate()
        {
            if (ConnectionString.IsEmpty())
            {
                ShowingException(nameof(ConnectionString));
            }
        }
    }
}
