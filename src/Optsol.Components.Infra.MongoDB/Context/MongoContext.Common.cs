using MongoDB.Driver;

namespace Optsol.Components.Infra.MongoDB.Context
{
    public partial class MongoContext
    {
        private void Configure()
        {
            var mongoClintIsNull = MongoClient != null;
            if (mongoClintIsNull)
            {
                return;
            }

            MongoClient = new MongoClient(_mongoSettings.ConnectionString);

            _database = MongoClient.GetDatabase(_mongoSettings.DatabaseName);
        }
    }
}
