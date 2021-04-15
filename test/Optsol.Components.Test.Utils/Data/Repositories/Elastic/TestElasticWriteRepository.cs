using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.ElasticSearch.Context;
using Optsol.Components.Infra.ElasticSearch.Repositories;
using Optsol.Components.Test.Utils.Entity.Entities;
using System;

namespace Optsol.Components.Test.Utils.Data.Repositories.Elastic
{
    public class TestElasticWriteRepository : ElasticRepository<TestEntity, Guid>, ITestElasticWriteRepository
    {
        public TestElasticWriteRepository(ElasticContext context, ILoggerFactory logger) 
            : base(context, logger)
        {
        }
    }
}
