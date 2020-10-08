using System;
using Flunt.Notifications;

namespace Optsol.Sdk.Domain
{
    public class Entity<TKey> : Notifiable, IEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public DateTime CreateDate { get; protected set; }
    }
}
