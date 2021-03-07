using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Domain.Entities;
using Optsol.Components.Infra.Data.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Optsol.Components.Shared.Extensions.PredicateBuilderExtensions;

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

            LambdaExpression expression = null;
            var parametrer = Expression.Parameter(typeof(TEntity), "entity");

            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                expression = CreateInitialFilter(expression);

                builder
                    .Property(entity => ((IDeletable)entity).IsDeleted)
                    .IsRequired();

                builder.Property(entity => ((IDeletable)entity).DeletedDate);

                var member = Expression.Property(parametrer, "IsDeleted");
                var constant = Expression.Constant(false);
                var body = Expression.Equal(member, constant);

                var deletableExpression = Expression.Lambda<Func<TEntity, bool>>(body, parametrer);

                expression = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(expression.Body, deletableExpression.Body), deletableExpression.Parameters);
            }

            if (typeof(TEntity).GetInterfaces().Contains(typeof(ITenant<TKey>)))
            {
                expression = CreateInitialFilter(expression);

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
                    var member = Expression.Property(parametrer, "TenantId");
                    var constant = Expression.Constant(_tenantProvider.GetTenantId());
                    var body = Expression.Equal(member, constant);

                    var deletableExpression = Expression.Lambda<Func<TEntity, bool>>(body, parametrer);

                    expression = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(expression.Body, deletableExpression.Body), deletableExpression.Parameters);
                }
            }

            var hasExpressionFilter = expression != null;
            if (hasExpressionFilter)
            {
                builder.HasQueryFilter(expression);
            }
        }

        private LambdaExpression CreateInitialFilter(LambdaExpression expression)
        {
            return expression ?? PredicateBuilder.True<TEntity>();
        }
    }
}
