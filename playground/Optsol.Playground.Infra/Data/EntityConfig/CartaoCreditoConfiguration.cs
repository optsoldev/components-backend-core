using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Infra.Data.EntityConfig
{
    public class CartaoCreditoConfiguration : EntityConfigurationBase<CartaoCreditoEntity, Guid>
    {
        public override void Configure(EntityTypeBuilder<CartaoCreditoEntity> builder)
        {
            builder.ToTable("CartaoCredito");

            builder.Property(entity => entity.NomeCliente)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(entity => entity.Numero)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(entity => entity.CodigoVerificacao)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(entity => entity.Validade)
                .HasMaxLength(200)
                .IsRequired();

            base.Configure(builder);
        }
    }
}
