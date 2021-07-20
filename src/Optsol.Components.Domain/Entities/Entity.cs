using FluentValidation;
using FluentValidation.Results;
using Optsol.Components.Domain.Notifications.Contracts;
using Optsol.Components.Shared.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Domain.Entities
{
    public abstract class Entity : Notifiable<Notification>, IEntity
    {
        public DateTime CreatedDate { get; protected set; }

        public abstract void Validate();

        public void AddNotifications(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                foreach (var failure in validationResult.Errors)
                {
                    AddNotification(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }
    }

    public class Entity<TKey> : Entity, IEntity<TKey>
    {
        public TKey Id { get; protected set; }

        public override void Validate()
        {
            var validator = new EntityContract();
            var resultOfValidation = validator.Validate(this);

            AddNotifications(resultOfValidation);
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
