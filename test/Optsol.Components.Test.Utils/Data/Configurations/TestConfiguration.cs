using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Infra.Data;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data.Configurations
{
    public class TestConfiguration : EntityConfigurationBase<TestEntity, Guid>
    {
        public override void Configure(EntityTypeBuilder<TestEntity> builder)
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
}