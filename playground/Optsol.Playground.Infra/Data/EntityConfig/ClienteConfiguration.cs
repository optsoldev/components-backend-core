using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Infra.Data.EntityConfig
{
    public class ClienteConfiguration : EntityConfigurationBase<ClienteEntity, Guid>
    {
        public override void Configure(EntityTypeBuilder<ClienteEntity> builder)
        {
            builder.ToTable("Cliente");

            builder.OwnsOne(entity => entity.Email, entity =>
            {
                entity.Property(a => a.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Ignore(a => a.Notifications);

            });

            builder.OwnsOne(entity => entity.Nome, entity =>
            {
                entity.Property(a => a.Nome)
                    .HasColumnName("Nome")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(a => a.SobreNome)
                    .HasColumnName("Sobrenome")
                   .HasMaxLength(200)
                   .IsRequired();

                entity.Ignore(a => a.Notifications);

            });

            builder.Property(entity => entity.Ativo);

            builder
                .HasMany(entity => entity.Cartoes)
                .WithOne(src => src.Cliente)
                .HasForeignKey(src => src.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}
