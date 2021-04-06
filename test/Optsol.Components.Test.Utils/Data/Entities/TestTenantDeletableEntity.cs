using Flunt.Validations;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Test.Utils.Contracts;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using System;


namespace Optsol.Components.Test.Utils.Data.Entities
{
    public class TestTenantDeletableEntity : AggregateRoot, ITenant, IDeletable
    {

        public bool IsDeleted { get; private set; }

        public DateTime? DeletedDate { get; private set; }

        public NomeValueObject Nome { get; private set; }

        public EmailValueObject Email { get; private set; }

        public bool Ativo { get; private set; }

        public Guid TenantId { get; private set; }

        public TestTenantDeletableEntity()
        {
        }

        public TestTenantDeletableEntity(Guid id, Guid tenantId, NomeValueObject nome, EmailValueObject email)
            : this(tenantId, nome, email)
        {
            Id = id;
        }

        public TestTenantDeletableEntity(Guid tenantId, NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;

            Validate();

            Ativo = false;
            TenantId = tenantId;
        }

        public void InserirNome(NomeValueObject nomeValueObject)
        {
            Nome = nomeValueObject;
        }

        public override void Validate()
        {
            AddNotifications(new TestTenantDeletableEntityContract(this));

            AddNotifications(Nome, Email);

            base.Validate();
        }

        public void SetTenantId(Guid tenantId)
        {
            TenantId = tenantId;
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedDate = DateTime.Now;
        }
    }
}