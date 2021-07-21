using Optsol.Components.Domain.Entities;
using Optsol.Components.Test.Utils.Contracts;
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

        public TestTenantEntity(Guid id, Guid tenantId, NomeValueObject nome, EmailValueObject email)
            : this(tenantId, nome, email)
        {
            Id = id;
        }

        public TestTenantEntity(Guid tenantId, NomeValueObject nome, EmailValueObject email)
        {
            Nome = nome;
            Email = email;
            TenantId = tenantId;
            Ativo = false;
        }

        public void InserirNome(NomeValueObject nomeValueObject)
        {
            Nome = nomeValueObject;
        }

        public override void Validate()
        {
            var validator = new TestTenantEntityContract();
            var resultOfValidation = validator.Validate(this);
            
            AddNotifications(resultOfValidation);

            base.Validate();
        }

        public void SetTenantId(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
