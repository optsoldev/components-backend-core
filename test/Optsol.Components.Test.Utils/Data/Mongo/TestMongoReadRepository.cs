using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Infra.MongoDB.Repository;
using Optsol.Components.Test.Utils.Data.Mongo;
using Optsol.Components.Test.Utils.Entity;
using System;

namespace Optsol.Components.Test.Integration.Infra.MongoDB
{
    public class TestMongoReadRepository : MongoRepository<TestEntity, Guid>, ITestMongoReadRepository
    {
        public TestMongoReadRepository(MongoContext context, ILogger<MongoRepository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }
}