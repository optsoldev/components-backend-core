using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Data.Context
{
    public class TestMongoContext : MongoContext
    {
        public TestMongoContext(MongoSettings mongoSettings) 
            : base(mongoSettings)
        {
        }
    }
}
