using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Optsol.Components.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Optsol.Components.Shared.Extensions.PredicateBuilderExtensions;

namespace Optsol.Components.Infra.Data
{
    public class EntityConfigurationBase<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TKey>
    {

        public EntityConfigurationBase()
        {
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Ignore(entity => entity.Notifications);
            builder.Ignore(entity => entity.Valid);
            builder.Ignore(entity => entity.Invalid);

            builder
                .Property(entity => entity.CreatedDate)
                .HasColumnName(nameof(Entity<TKey>.CreatedDate))
                .HasColumnType("datetime")
                .IsRequired();

            BuildQueryKey(builder);

            LambdaExpression expression = null;
            var parametrer = Expression.Parameter(typeof(TEntity), "entity");
            expression = ConfigureDeletableParams(builder, expression, parametrer);
            expression = ConfigureTenantParams(builder, expression, parametrer);

            BuildQueryFilter(builder, expression);
        }

        private static void BuildQueryKey(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).ValueGeneratedNever();

            if (typeof(TEntity).GetInterfaces().Contains(typeof(ITenant<TKey>)))
            {
                builder.Property(entity => ((ITenant<TKey>)entity).TenantId).IsRequired();
            }
        }

        private static void BuildQueryFilter(EntityTypeBuilder<TEntity> builder, LambdaExpression expression)
        {
            var hasExpressionFilter = expression != null;
            if (hasExpressionFilter)
            {
                builder.HasQueryFilter(expression);
            }
        }

        private LambdaExpression ConfigureTenantParams(EntityTypeBuilder<TEntity> builder, LambdaExpression expression, ParameterExpression parametrer)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(ITenant<TKey>)))
            {
                expression = CreateInitialFilter(expression);

                builder
                    .Property(entity => ((ITenant<TKey>)entity).TenantId)
                    .IsRequired();

                var tenantExpression = CreateExpression(parametrer, "TenantId", "6025384C-3CF7-4310-AFA9-244507D1FE9B");

                expression = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(expression.Body, tenantExpression.Body), tenantExpression.Parameters);
            }

            return expression;
        }

        private static LambdaExpression ConfigureDeletableParams(EntityTypeBuilder<TEntity> builder, LambdaExpression expression, ParameterExpression parametrer)
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IDeletable)))
            {
                expression = CreateInitialFilter(expression);

                builder
                    .Property(entity => ((IDeletable)entity).IsDeleted)
                    .IsRequired();

                builder.Property(entity => ((IDeletable)entity).DeletedDate);

                var deletableExpression = CreateExpression(parametrer, "IsDeleted", false);

                expression = Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(expression.Body, deletableExpression.Body), deletableExpression.Parameters);
            }

            return expression;
        }

        private static Expression<Func<TEntity, bool>> CreateExpression<T>(ParameterExpression parametrer, string propertyName, T value)
        {
            var member = Expression.Property(parametrer, propertyName);
            var constant = Expression.Constant(value);
            var body = Expression.Equal(member, constant);

            return Expression.Lambda<Func<TEntity, bool>>(body, parametrer);
        }

        private static LambdaExpression CreateInitialFilter(LambdaExpression expression)
        {
            return expression ?? PredicateBuilder.True<TEntity>();
        }
    }
}
