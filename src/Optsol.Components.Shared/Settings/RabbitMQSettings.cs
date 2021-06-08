using System;

namespace Optsol.Components.Shared.Settings
{
    public class RabbitMQSettings : BaseSettings
    {
        public string HostName { get; set; }

        public string ExchangeName { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public override void Validate()
        {
            if (HostName.IsEmpty())
            {
                ShowingException(nameof(HostName));
            }

            if (ExchangeName.IsEmpty())
            {
                ShowingException(nameof(ExchangeName));
            }
        }
    }
}
