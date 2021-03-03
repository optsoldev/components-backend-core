using Optsol.Components.Infra.MongoDB.Context;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Data.Contexts
{
    public class MongoContext : Infra.MongoDB.Context.MongoContext
    {
        public MongoContext(MongoSettings mongoSettings) 
            : base(mongoSettings)
        {
        }

        //public override void Dispose()
        //{
        //    base.Dispose();

        //    _database.Client.DropDatabase(_mongoSettings.DatabaseName);
        //}
    }
}
