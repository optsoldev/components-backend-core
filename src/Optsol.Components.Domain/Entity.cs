using System;
using Flunt.Notifications;
using Flunt.Validations;

namespace Optsol.Components.Domain
{
    public class Entity<TKey> : Notifiable, IEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public DateTime CreateDate { get; protected set; }

        public virtual void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsLowerThan(CreateDate, DateTime.Now, "CreateDate", "A Data de criação não pode ser maior que a data atual"));
        }
    }
}
