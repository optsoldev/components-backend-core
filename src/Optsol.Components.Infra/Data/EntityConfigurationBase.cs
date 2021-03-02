using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data.Provider;
using System.Collections.Generic;
using System.Linq;

namespace Optsol.Components.Infra.Data
{
    public class EntityConfigurationBase<TEntity, TKey> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TKey>
    {

        protected readonly ITenantProvider<TKey> _tenantProvider;

        public EntityConfigurationBase(ITenantProvider<TKey> tenantProvider = null)
        {
            _tenantProvider = tenantProvider;
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Ignore(entity => entity.Notifications);
            builder.Ignore(entity => entity.Invalid);
            builder.Ignore(entity => entity.Valid);

            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).ValueGeneratedNever();

            builder
                .Property(entity => entity.CreatedDate)
                .HasColumnName(nameof(Entity<TKey>.CreatedDate))
                .HasColumnType("datetime")
                .IsRequired();

            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                builder
                    .Property(entity => ((IDeletable)entity).IsDeleted)
                    .IsRequired();

                builder.Property(entity => ((IDeletable)entity).DeletedDate);

                builder.HasQueryFilter(entity => !((IDeletable)entity).IsDeleted);
            }

            if (typeof(TEntity).GetInterfaces().Contains(typeof(ITenant<TKey>)))
            {
                builder
                    .Property(entity => ((ITenant<TKey>)entity).TenantId)
                    .IsRequired();
                
                builder
                    .HasIndex("TenantId")
                    .IsUnique()
                    .HasDatabaseName($"{typeof(TEntity).Name}TenantIndex");

                var tenantProviderNotIsNull = _tenantProvider != null;
                if (tenantProviderNotIsNull)
                {
                    builder.HasQueryFilter(entity => EqualityComparer<TKey>.Default.Equals(((ITenant<TKey>)entity).TenantId, _tenantProvider.GetTenantId()));
                }
            }
        }
    }
}
