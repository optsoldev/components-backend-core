using System;
using Flunt.Notifications;
using Flunt.Validations;

namespace Optsol.Components.Domain.Entities
{
    public abstract class Entity : Notifiable, IEntity
    {
        public DateTime CreatedDate { get; protected set; }

        public abstract void Validate();
    }

    public class Entity<TKey> : Entity, IEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsLowerThan(CreatedDate, DateTime.Now, "CreationDate", "A Data de criação não pode ser maior que a data atual"));
        }
    }

    public class EntityGuid : Entity<Guid>
    {
        public EntityGuid() : base()
        {
            base.CreatedDate = DateTime.Now;
            base.Id = Guid.NewGuid();
        }
    }
}
