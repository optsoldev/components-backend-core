using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Data.Contexts
{
    public class MongoContext : Infra.MongoDB.Context.MongoContext
    {
        public MongoContext(MongoSettings mongoSettings, ILoggerFactory logger)
            : base(mongoSettings, logger)
        {
        }
    }
}
