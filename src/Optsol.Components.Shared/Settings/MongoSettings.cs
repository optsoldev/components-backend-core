using System;

namespace Optsol.Components.Shared.Settings
{
    public class MongoSettings : BaseSettings
    {
        public string ConnectionString { get; set; }
        
        public string DatabaseName { get; set; }

        public override void Validate()
        {
            if (ConnectionString.IsEmpty())
            {
                ShowingException(nameof(ConnectionString));
            }

            if (DatabaseName.IsEmpty())
            {
                ShowingException(nameof(DatabaseName));
            }
        }
    }
}