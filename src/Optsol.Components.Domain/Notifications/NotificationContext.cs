using Flunt.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Domain.Notifications
{
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;

        public IReadOnlyCollection<Notification> Notifications => _notifications;

        public bool HasNotifications => _notifications.Any();

        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(string key, string message)
        {
            _notifications.Add(new Notification(key, message));
        }

        public void AddNotifications(Notification data)
        {
            _notifications.Add(data);
        }

        public void AddNotifications(IReadOnlyCollection<Notification> data) 
        {
            _notifications.AddRange(data);
        }
    }
}
