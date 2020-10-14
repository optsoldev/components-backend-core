using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Domain;

namespace Optsol.Components.Infra.Data
{
    public class EntityConfigurationBase<TEntity, TKey> :
        IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TKey>
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Ignore(entity => entity.Notifications);
            builder.Ignore(entity => entity.Invalid);
            builder.Ignore(entity => entity.Valid);
            
            builder.HasKey(entity => entity.Id);
            builder
                .Property(entity => entity.CreateDate)
                .HasColumnName(nameof(Entity<TKey>.CreateDate))
                .HasColumnType("datetime")
                .IsRequired();
        }
    }
}
