using MongoDB.Driver;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.MongoDB.Context
{
    public class MongoContext : IDisposable
    {
        protected IMongoDatabase _database;

        protected readonly List<Func<Task>> _commands;

        protected readonly MongoSettings _mongoSettings;

        public IClientSessionHandle Session { get; set; }

        public MongoClient MongoClient { get; set; }

        public MongoContext(MongoSettings mongoSettings)
        {
            _mongoSettings = mongoSettings;

            _commands = new List<Func<Task>>();

            Configure();
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            Configure();

            return _database.GetCollection<T>(name);
        }

        public async Task<int> SaveChangesAsync()
        {
            Configure();

            var countSaveTasks = 0;

            using (Session = await MongoClient.StartSessionAsync())
            {
                Session.StartTransaction();

                var commandsTasks = _commands.Select(execute => execute());

                await Task.WhenAll(commandsTasks);

                countSaveTasks = _commands.Count;

                _commands.Clear();

                await Session.CommitTransactionAsync();
            }

            return countSaveTasks;
        }
        
        public void AddCommand(Func<Task> command)
        {
            _commands.Add(command);
        }
        
        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

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
