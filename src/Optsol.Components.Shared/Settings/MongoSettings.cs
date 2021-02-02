using System;

namespace Optsol.Components.Shared.Settings
{
    public class MongoSettings
    {
        public string ConnectionString { get; set; }
        
        public string DatabaseName { get; set; }

        public void Validate()
        {
            var connectionStringIsNullOrEmpty = string.IsNullOrEmpty(ConnectionString);
            if (connectionStringIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }

            var databaseNameIsNullOrEmpty = string.IsNullOrEmpty(DatabaseName);
            if (databaseNameIsNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(DatabaseName));
            }
        }
    }
}