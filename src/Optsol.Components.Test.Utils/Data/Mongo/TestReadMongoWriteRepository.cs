using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Infra.MongoDB.Repository;
using System;

namespace Optsol.Components.Test.Utils.Entity.Mongo
{
    public class TestReadMongoWriteRepository : MongoRepository<TestEntity, Guid>, ITestMongoReadRepository
    {
        public TestReadMongoWriteRepository(MongoContext context, ILogger<MongoRepository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }
}
