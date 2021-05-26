using System;
using System.Collections.Generic;

namespace Optsol.Components.Shared.Settings
{
    public class StorageSettings : BaseSettings
    {
        public string ConnectionString { get; set; }

        public override void Validate()
        {
            var connectionIsNull = string.IsNullOrEmpty(ConnectionString);
            if (connectionIsNull)
            {
                throw new ApplicationException(nameof(ConnectionString));
            }
        }
    }
}
