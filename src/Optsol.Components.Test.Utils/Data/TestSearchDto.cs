using System;
using System.Linq;
using System.Linq.Expressions;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Extensions;

namespace Optsol.Components.Test.Utils.Data
{

    public class TestSearchDto : ISearch<TestEntity>, IOrderBy<TestEntity>, IInclude<TestEntity>
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }

        public Expression<Func<TestEntity, bool>> GetSearcher()
        {
            var exp = PredicateBuilderExtensions.True<TestEntity>();

            var nomeIsNotNull = !string.IsNullOrEmpty(Nome);
            if (nomeIsNotNull)
            {
                exp.And(entity => entity.Nome.Nome.Contains(Nome));
            }

            var sobreNomeIsNotNull = !string.IsNullOrEmpty(SobreNome);
            if (sobreNomeIsNotNull)
            {
                exp.And(entity => entity.Nome.SobreNome.Contains(SobreNome));
            }

            return exp;
        }

        public Func<IQueryable<TestEntity>, IOrderedQueryable<TestEntity>> GetOrderBy()
        {
            return entity => entity.OrderBy(o => o.Nome.Nome);
        }
        public Func<IQueryable<TestEntity>, IQueryable<TestEntity>> GetInclude()
        {
            return null;
        }
    }

    public class TestSearchOnlyDto : ISearch<TestEntity>
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }

        public Expression<Func<TestEntity, bool>> GetSearcher()
        {
            var exp = PredicateBuilderExtensions.True<TestEntity>();

            var nomeIsNotNull = !string.IsNullOrEmpty(Nome);
            if (nomeIsNotNull)
            {
                exp.And(entity => entity.Nome.Nome.Contains(Nome));
            }

            var sobreNomeIsNotNull = !string.IsNullOrEmpty(SobreNome);
            if (sobreNomeIsNotNull)
            {
                exp.And(entity => entity.Nome.SobreNome.Contains(SobreNome));
            }

            return exp;
        }
    }
}
