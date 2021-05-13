using Optsol.Components.Infra.Data.Pagination;
using Optsol.Playground.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Optsol.Components.Shared.Extensions.PredicateBuilderExtensions;

namespace Optsol.Playground.Application.Searchs
{
    public class ClienteSearchDto : ISearch<ClienteEntity>, IOrderBy<ClienteEntity>
    {
        public string Nome { get; set; }

        public Expression<Func<ClienteEntity, bool>> GetSearcher()
        {
            var expression = PredicateBuilder.True<ClienteEntity>();

            expression = expression
                .And(_ => _.Nome.Nome.Contains(Nome) || _.Nome.SobreNome.Contains(Nome));

            return expression;
            
        }

        public Func<IQueryable<ClienteEntity>, IOrderedQueryable<ClienteEntity>> GetOrderBy()
        {
            return query => query.OrderByDescending(_ => _.CreatedDate);
        }
    }
}
