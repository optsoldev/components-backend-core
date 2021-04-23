using System;

namespace Optsol.Components.Shared.Settings
{
    public class MongoSettings : BaseSettings
    {
        public string ConnectionString { get; set; }
        
        public string DatabaseName { get; set; }

        public override void Validate()
        {
            var connectionStringIsNullOrEmpty = string.IsNullOrEmpty(ConnectionString);
            if (connectionStringIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(ConnectionString));
            }

            var databaseNameIsNullOrEmpty = string.IsNullOrEmpty(DatabaseName);
            if (databaseNameIsNullOrEmpty)
            {
                throw new ApplicationException(nameof(DatabaseName));
            }
        }
    }
}