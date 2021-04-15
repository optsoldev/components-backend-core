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
        public ElasticClient ElasticClient { get; protected set; }

        protected readonly ElasticSearchSettings _elasticSearchSettings;
        protected readonly List<Func<Task>> _commands;

        public ElasticContext(ElasticSearchSettings elasticSearchSettings)
        {
            _elasticSearchSettings = elasticSearchSettings ?? throw new ArgumentNullException(nameof(elasticSearchSettings));

            _commands = new List<Func<Task>>();

            ConfigureClient();
        }

        public async Task<int> SaveChangesAsync()
        {
            var countSaveTasks = 0;

            var commandsTasks = _commands.Select(execute => execute());

            await Task.WhenAll(commandsTasks);

            countSaveTasks = _commands.Count;

            _commands.Clear();

            return countSaveTasks;
        }

        public void AddCommand(Func<Task> command)
        {
            _commands.Add(command);
        }

        public void Dispose()
        {
            ElasticClient = null;
            GC.SuppressFinalize(this);
        }

        private void ConfigureClient()
        {
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
                if(hasUserNamePassword)
                {
                    settings = settings.BasicAuthentication(_elasticSearchSettings.UserName, _elasticSearchSettings.Password);
                }

                ElasticClient = new ElasticClient(settings);
            }
        }

        public void CreateIndex(string indexName)
        {
            var indexNotExists = !ElasticClient.Indices.Exists(indexName.ToLower()).Exists;
            if (indexNotExists)
            {
                ElasticClient.Indices.Create(indexName.ToLower());
            }
        }
    }
}
