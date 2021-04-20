using System;

namespace Optsol.Components.Shared.Settings
{
    public class RedisSettings : BaseSettings
    {
        public string ConnectionString { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new ArgumentNullException(nameof(ConnectionString));
            }
        }
    }
}
