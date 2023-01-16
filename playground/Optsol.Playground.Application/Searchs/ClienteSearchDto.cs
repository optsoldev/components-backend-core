using Optsol.Components.Infra.Data.Pagination;
using Optsol.Playground.Domain.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using Optsol.Playground.Domain.Clientes;
using static Optsol.Components.Shared.Extensions.PredicateBuilderExtensions;

namespace Optsol.Playground.Application.Searchs
{
    public class ClienteSearchDto : ISearch<Cliente>, IOrderBy<Cliente>
    {
        public string Nome { get; set; }

        public Expression<Func<Cliente, bool>> GetSearcher()
        {
            var expression = PredicateBuilder.True<Cliente>();

            expression = expression
                .And(_ => _.Nome.Nome.Contains(Nome) || _.Nome.SobreNome.Contains(Nome));

            return expression;
            
        }

        public Func<IQueryable<Cliente>, IOrderedQueryable<Cliente>> GetOrderBy()
        {
            return query => query.OrderByDescending(_ => _.CreatedDate);
        }
    }
}
