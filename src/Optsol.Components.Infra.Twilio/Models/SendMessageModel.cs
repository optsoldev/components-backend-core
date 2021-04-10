using Flunt.Notifications;
using Optsol.Components.Infra.Twilio.Validators;

namespace Optsol.Components.Infra.Twilio.Models
{
    public class SendMessageModel<T> : Notifiable<Notification>
        where T : class
    {
        public SendMessageModel(string from, string to, T data)
        {
            From = from;
            To = to;
            Data = data;

            AddNotifications(new SendMessageModelContract<T>(this));
        }

        public string From { get; private set; }

        public string To { get; private set; }

        public T Data { get; private set; }

    }
}
