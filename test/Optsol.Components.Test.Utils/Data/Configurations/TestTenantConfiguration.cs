using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.Data.Provider;
using Optsol.Components.Test.Utils.Data.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data.Configurations
{
    public class TestTenantConfiguration : EntityConfigurationBase<TestTenantEntity, Guid>
    {
        public TestTenantConfiguration(ITenantProvider tenantProvider)
            : base(tenantProvider)
        {

        }

        public override void Configure(EntityTypeBuilder<TestTenantEntity> builder)
        {
            builder.OwnsOne(valueObject => valueObject.Nome)
               .Ignore(nome => nome.Notifications)
               .Ignore(nome => nome.IsValid);

            builder.OwnsOne(valueObject => valueObject.Email)
                .Ignore(nome => nome.Notifications)
                .Ignore(nome => nome.IsValid);

            builder
               .OwnsOne(valueObject => valueObject.Nome)
               .Property(prop => prop.Nome)
               .HasColumnName("Nome")
               .HasMaxLength(35)
               .IsRequired();

            builder
                .OwnsOne(valueObject => valueObject.Nome)
                .Property(prop => prop.SobreNome)
                .HasColumnName("SobreNome")
                .HasMaxLength(35)
                .IsRequired();

            builder
                .OwnsOne(valueObject => valueObject.Email)
                .Property(prop => prop.Email)
                .HasColumnName("Email")
                .HasMaxLength(35)
                .IsRequired();

            builder
                .Property(entity => entity.Ativo)
                .IsRequired();

            base.Configure(builder);
        }
    }

    public class TestTenantDeletableConfiguration : EntityConfigurationBase<TestTenantDeletableEntity, Guid>
    {
        public TestTenantDeletableConfiguration(ITenantProvider tenantProvider)
            : base(tenantProvider)
        {

        }

        public override void Configure(EntityTypeBuilder<TestTenantDeletableEntity> builder)
        {
            builder.OwnsOne(valueObject => valueObject.Nome)
               .Ignore(nome => nome.Notifications)
               .Ignore(nome => nome.IsValid);

            builder.OwnsOne(valueObject => valueObject.Email)
                .Ignore(nome => nome.Notifications)
                .Ignore(nome => nome.IsValid);

            builder
               .OwnsOne(valueObject => valueObject.Nome)
               .Property(prop => prop.Nome)
               .HasColumnName("Nome")
               .HasMaxLength(35)
               .IsRequired();

            builder
                .OwnsOne(valueObject => valueObject.Nome)
                .Property(prop => prop.SobreNome)
                .HasColumnName("SobreNome")
                .HasMaxLength(35)
                .IsRequired();

            builder
                .OwnsOne(valueObject => valueObject.Email)
                .Property(prop => prop.Email)
                .HasColumnName("Email")
                .HasMaxLength(35)
                .IsRequired();

            builder
                .Property(entity => entity.Ativo)
                .IsRequired();

            base.Configure(builder);
        }
    }

    public class TenantConfiguration : EntityConfigurationBase<TenantEntity, Guid>
    {
        public override void Configure(EntityTypeBuilder<TenantEntity> builder)
        {
            builder
                .Property(entity => entity.Host)
                .IsRequired();

            builder
                .Property(entity => entity.Name)
                .IsRequired();

            base.Configure(builder);
        }
    }

}