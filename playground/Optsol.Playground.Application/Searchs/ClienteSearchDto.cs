using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Shared.Extensions;
using Optsol.Playground.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Optsol.Components.Shared.Extensions.PredicateBuilderExtensions;

namespace Optsol.Playground.Application.Searchs
{
    public class ClienteSearchDto : ISearch<ClienteEntity>, IOrderBy<ClienteEntity>, IInclude<ClienteEntity>
    {
        public string Nome { get; set; }

        public Expression<Func<ClienteEntity, bool>> GetSearcher()
        {
            var expression = PredicateBuilder.True<ClienteEntity>();

            expression = expression.And(_ => _.Nome.Constains(Nome));

            return expression;
            
        }

        public Func<IQueryable<ClienteEntity>, IQueryable<ClienteEntity>> GetInclude()
        {
            throw new NotImplementedException();
        }

        public Func<IQueryable<ClienteEntity>, IOrderedQueryable<ClienteEntity>> GetOrderBy()
        {
            return query => query.OrderByDescending(_ => _.CreatedDate);
        }
    }
}
