using FluentValidation;
using Optsol.Components.Domain.Entities;
using System;

namespace Optsol.Components.Domain.Notifications.Contracts
{
    public class EntityContract : AbstractValidator<Entity>
    {
        public EntityContract()
        {
            RuleFor(entity => entity.CreatedDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("A Data de criação não pode ser maior que a data atual");
        }
    }
}
