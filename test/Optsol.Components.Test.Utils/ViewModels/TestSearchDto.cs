using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Infra.Data.Pagination;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Utils.Contracts;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;
using static Optsol.Components.Shared.Extensions.PredicateBuilderExtensions;

namespace Optsol.Components.Test.Utils.ViewModels
{

    public class TestSearchDto : BaseModel, ISearch<TestEntity>, IOrderBy<TestEntity>, IInclude<TestEntity>
    {
        public string Nome { get; set; }

        public string SobreNome { get; set; }

        public Expression<Func<TestEntity, bool>> GetSearcher()
        {
            var exp = PredicateBuilder.True<TestEntity>();

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

        public override void Validate()
        {
            //TODO: REVER
            //AddNotifications(new TestSearchDtoContract(this));
        }
    }

    public class TestSearchOnlyDto : BaseModel, ISearch<TestEntity>
    {
        public string Nome { get; set; }

        public string SobreNome { get; set; }

        public Expression<Func<TestEntity, bool>> GetSearcher()
        {
            var exp = PredicateBuilder.True<TestEntity>();

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

        public override void Validate()
        {
            //TODO: REVER
            //AddNotifications(new TestSearchOnlyDtoContract(this));
        }
    }
}
