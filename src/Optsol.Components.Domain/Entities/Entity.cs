using Flunt.Notifications;
using Optsol.Components.Domain.Notifications.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Domain.Entities
{
    public abstract class Entity : Notifiable<Notification>, IEntity
    {
        public DateTime CreatedDate { get; protected set; }

        public abstract void Validate();

        public void AddNotifications(IList<Entity> entities)
        {
            base.AddNotifications(entities.SelectMany(s => s.Notifications) as IReadOnlyList<Notification>);
        }
    }

    public class Entity<TKey> : Entity, IEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public override void Validate()
        {
            AddNotifications(new EntityContract(this));
        }
    }

    public class EntityGuid : Entity<Guid>
    {
        public EntityGuid() : base()
        {
            CreatedDate = DateTime.Now;
            Id = Guid.NewGuid();
        }
    }
}
