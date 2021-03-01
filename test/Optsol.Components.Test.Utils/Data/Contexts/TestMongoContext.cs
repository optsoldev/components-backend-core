using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Data.Contexts
{
    public class TestMongoContext : MongoContext
    {
        public TestMongoContext(MongoSettings mongoSettings) 
            : base(mongoSettings)
        {
        }
    }
}
