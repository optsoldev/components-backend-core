using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.ElasticSearch.Context;
using Optsol.Components.Infra.ElasticSearch.Repositories;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data.Repositories.Elastic
{
    public class TestElasticReadRepository : ElasticRepository<TestEntity, Guid>, ITestElasticReadRepository
    {
        public TestElasticReadRepository(ElasticContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }
}