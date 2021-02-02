using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Infra.MongoDB.Repository;
using Optsol.Components.Test.Utils.Entity;
using Optsol.Components.Test.Utils.Entity.Mongo;
using System;

namespace Optsol.Components.Test.Integration.Infra.MongoDB
{
    public class TestReadMongoRepository : MongoRepository<TestEntity, Guid>, ITestMongoReadRepository
    {
        public TestReadMongoRepository(MongoContext context, ILogger<MongoRepository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }
}