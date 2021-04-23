using System;

namespace Optsol.Components.Shared.Settings
{
    public class RabbitMQSettings : BaseSettings
    {
        public string HostName { get; set; }
        
        public int Port { get; set; }
        
        public string UserName { get; set; }
        
        public string Password { get; set; }

        public string ExchangeName { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(HostName))
            {
                throw new NullReferenceException(nameof(HostName));
            }

            if (string.IsNullOrEmpty(ExchangeName))
            {
                throw new NullReferenceException(nameof(ExchangeName));
            }
        }
    }
}
