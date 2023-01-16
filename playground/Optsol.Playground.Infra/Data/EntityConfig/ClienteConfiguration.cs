using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Infra.Data;
using Optsol.Playground.Domain.Clientes;
using Optsol.Playground.Domain.Entities;

namespace Optsol.Playground.Infra.Data.EntityConfig
{
    public class ClienteConfiguration : EntityConfigurationBase<Cliente, Guid>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("Cliente");

            builder.Property(entity => entity.Id).HasColumnName("ClienteId");

            builder.OwnsOne(entity => entity.Email, entity =>
            {
                entity.Property(a => a.Email)
                    .HasColumnName("Email")
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Ignore(a => a.Notifications);
                entity.Ignore(a => a.CreatedDate);

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
                entity.Ignore(a => a.CreatedDate);

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

    public class ClientePessoaFisicaConfiguration : EntityConfigurationBase<ClientePessoaFisica, Guid>
    {
        public override void Configure(EntityTypeBuilder<ClientePessoaFisica> builder)
        {
            builder.ToTable("ClientePessoaFisica");

            builder.Property(entity => entity.Documento);

            //base.Configure(builder);
        }
    }

    public class ClientePessoaJuridicaConfiguration : EntityConfigurationBase<ClientePessoaJuridica, Guid>
    {
        public override void Configure(EntityTypeBuilder<ClientePessoaJuridica> builder)
        {
            builder.ToTable("ClientePessoaJuridica");

            builder.Property(entity => entity.NumeroCnpj);

            //base.Configure(builder);
        }
    }
}
