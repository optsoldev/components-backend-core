using Microsoft.Extensions.Logging;
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
        private bool _disposed = false;

        private readonly ILogger _logger;

        protected IMongoDatabase _database;

        protected readonly List<Func<Task>> _commands;

        protected readonly MongoSettings _mongoSettings;

        public IClientSessionHandle Session { get; set; }

        public MongoClient MongoClient { get; protected set; }

        public MongoContext(MongoSettings mongoSettings, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(MongoContext));
            _logger?.LogInformation("Inicializando MongoContext");

            _mongoSettings = mongoSettings ?? throw new ArgumentNullException(nameof(mongoSettings));

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!_disposed && disposing)
            {
                Session?.Dispose();
            }
            _disposed = true;
        }

        private void Configure()
        {
            var mongoClintNotNull = MongoClient != null;
            if (mongoClintNotNull)
            {
                return;
            }

            MongoClient = new MongoClient(_mongoSettings.ConnectionString);

            _database = MongoClient.GetDatabase(_mongoSettings.DatabaseName);
        }
    }
}
