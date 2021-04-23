using Microsoft.Extensions.Logging;
using Nest;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.ElasticSearch.Context
{
    public class ElasticContext : IDisposable
    {
        private bool _disposed = false;

        private readonly ILogger _logger;

        public ElasticClient ElasticClient { get; protected set; }

        protected readonly ElasticSearchSettings _elasticSearchSettings;

        protected readonly List<Func<Task>> _commands;

        public ElasticContext(ElasticSearchSettings elasticSearchSettings, ILoggerFactory logger)
        {
            _logger = logger?.CreateLogger(nameof(ElasticContext));
            _logger?.LogInformation("Inicializando ElasticContext");

            _elasticSearchSettings = elasticSearchSettings ?? throw new ArgumentNullException(nameof(elasticSearchSettings));

            _commands = new List<Func<Task>>();

            ConfigureClient();
        }

        public async Task<int> SaveChangesAsync()
        {
            _logger?.LogInformation($"Método: { nameof(SaveChangesAsync) }() Retorno: Task<int>");

            var countSaveTasks = 0;

            var commandsTasks = _commands.Select(execute => execute());

            await Task.WhenAll(commandsTasks);

            countSaveTasks = _commands.Count;

            _commands.Clear();

            return countSaveTasks;
        }

        public void AddCommand(Func<Task> command)
        {
            _logger?.LogInformation($"Método: { nameof(AddCommand) }()");

            _commands.Add(command);
        }
        
        public void CreateIndex(string indexName)
        {
            _logger?.LogInformation($"Método: { nameof(CreateIndex) }()");

            var indexNotExists = !ElasticClient.Indices.Exists(indexName.ToLower()).Exists;
            if (indexNotExists)
            {
                ElasticClient.Indices.Create(indexName.ToLower());
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _logger?.LogInformation($"Método: { nameof(Dispose) }()");

            if (!_disposed && disposing)
            {
                ElasticClient = null;
            }
            _disposed = true;
        }

        private void ConfigureClient()
        {
            _logger?.LogInformation($"Método: { nameof(ConfigureClient) }()");

            var clientIsNull = ElasticClient == null;
            if (clientIsNull)
            {
                var settings = new ConnectionSettings(new Uri(_elasticSearchSettings.Uri));

                var hasIndexName = _elasticSearchSettings.IndexName != null;
                if (hasIndexName)
                {
                    settings = settings.DefaultIndex(_elasticSearchSettings.IndexName);
                }

                var hasUserNamePassword = string.IsNullOrEmpty(_elasticSearchSettings.UserName) && string.IsNullOrEmpty(_elasticSearchSettings.Password);
                if (hasUserNamePassword)
                {
                    settings = settings.BasicAuthentication(_elasticSearchSettings.UserName, _elasticSearchSettings.Password);
                }

                ElasticClient = new ElasticClient(settings);
            }
        }
    }
}
