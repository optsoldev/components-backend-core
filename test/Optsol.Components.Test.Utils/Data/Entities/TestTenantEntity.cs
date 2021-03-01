using Flunt.Validations;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Test.Utils.Data.Entities.ValueObjecs;
using System;


namespace Optsol.Components.Test.Utils.Data.Entities
{
    public class TestTenantEntity : AggregateRoot, ITenant
    {
        public NomeValueObject Nome { get; private set; }
        public EmailValueObject Email { get; private set; }
        public bool Ativo { get; private set; }

        public Guid TenantId { get; private set; }

        public TestTenantEntity()
        {
        }

        public TestTenantEntity(Guid id, NomeValueObject nome, EmailValueObject email)
            : this(nome, email)
        {
            Id = id;
        }

        public TestTenantEntity(NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;

            Validate();

            Ativo = false;
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
    }
}
