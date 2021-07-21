using System.Collections.Generic;

namespace Optsol.Components.Shared.Notifications
{
    public interface INotifiable
    {
        void AddNotifications(IEnumerable<Notification> notifications);
    }
}
