using System;
using System.Linq;
using System.Linq.Expressions;
using Flunt.Validations;
using Optsol.Components.Application.DataTransferObjects;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Test.Utils.Entity;

namespace Optsol.Components.Test.Utils.Data
{

    public class TestSearchDto : BaseDataTransferObject, ISearch<TestEntity>, IOrderBy<TestEntity>, IInclude<TestEntity>
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

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Nome, nameof(Nome), "O nome do cliente não pode ser nulo")
                .IsNullOrEmpty(SobreNome, nameof(SobreNome), "O sobrenome do cliente não pode ser nulo")
                );
        }
    }

    public class TestSearchOnlyDto : BaseDataTransferObject, ISearch<TestEntity>
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

        public override void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(Nome, nameof(Nome), "O nome do cliente não pode ser nulo")
                .IsNullOrEmpty(SobreNome, nameof(SobreNome), "O sobrenome do cliente não pode ser nulo")
                );
        }
    }
}
