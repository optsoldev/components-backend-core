using Flunt.Validations;
using Optsol.Components.Domain.Entities;
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
            base.Validate();

            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Nome, "Nome", "O Nome não pode ser nulo")
                .IsNotNull(Email, "Email", "O Email não pode ser nulo")
                );

            var tenantIdIsNullOrEmpty = TenantId == Guid.Empty;
            if (tenantIdIsNullOrEmpty)
                AddNotification(nameof(TenantId), "O Tenant Id não pode ser nulo");

            if (Invalid)
                return;

            AddNotifications(Nome, Email);

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