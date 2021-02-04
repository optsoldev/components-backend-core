using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Infra.MongoDB.Repository;
using Optsol.Components.Test.Utils.Entity;
using System;

namespace Optsol.Components.Test.Utils.Data.Mongo
{
    public class TestMongoWriteRepository : MongoRepository<TestEntity, Guid>, ITestMongoWriteRepository
    {
        public TestMongoWriteRepository(MongoContext context, ILogger<MongoRepository<TestEntity, Guid>> logger) 
            : base(context, logger)
        {
        }
    }
}
