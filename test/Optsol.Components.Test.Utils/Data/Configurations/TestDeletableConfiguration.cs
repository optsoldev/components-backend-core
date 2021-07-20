using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Data.Entities;

namespace Optsol.Components.Test.Utils.Data.Configurations
{
    public class TestDeletableConfiguration : EntityConfigurationBase<TestDeletableEntity, Guid>
    {
        public override void Configure(EntityTypeBuilder<TestDeletableEntity> builder)
        {
            builder.OwnsOne(valueObject => valueObject.Nome)
               .Ignore(nome => nome.Notifications)
               .Ignore(nome => nome.Invalid)
               .Ignore(nome => nome.Valid);

            builder.OwnsOne(valueObject => valueObject.Email)
                .Ignore(nome => nome.Notifications)
                .Ignore(nome => nome.Invalid)
                .Ignore(nome => nome.Valid);

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
}