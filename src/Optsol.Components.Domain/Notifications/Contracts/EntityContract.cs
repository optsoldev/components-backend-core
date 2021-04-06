using Flunt.Validations;
using Optsol.Components.Domain.Entities;
using System;

namespace Optsol.Components.Domain.Notifications.Contracts
{
    public class EntityContract : Contract<Entity>
    {
        public EntityContract(Entity entity)
        {
            Requires()
                .IsLowerThan(entity.CreatedDate, DateTime.Now, "CreationDate", "A Data de criação não pode ser maior que a data atual");
        }
    }
}
