using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Optsol.Components.Domain.Data;
using Optsol.Components.Infra.Data;
using Optsol.Components.Infra.UoW;
using Optsol.Components.Shared.Exceptions;
using System;

namespace Microsoft.Extensions.DependencyInjection
{

    public class RepositoryOptions
    {
        private readonly IServiceCollection _services;

        internal bool InMemoryEnabled { get; private set; } = false;

        internal bool LoggingEnabled { get; private set; } = false;

        public RepositoryOptions(IServiceCollection services)
        {
            _services = services;
        }

        internal string MigrationsAssemblyName { get; private set; }

        internal string ConnectionString { get; private set; }

        public RepositoryOptions EnabledInMemory()
        {
            this.InMemoryEnabled = true;

            return this;
        }

        public RepositoryOptions EnabledLogging()
        {
            this.LoggingEnabled = true;

            return this;
        }

        public RepositoryOptions ConfigureMigrationsAssemblyName(string migrationsAssemblyName)
        {
            var isNullOrEmpty = string.IsNullOrEmpty(migrationsAssemblyName);
            if (isNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(migrationsAssemblyName));
            }

            MigrationsAssemblyName = migrationsAssemblyName;

            return this;
        }

        public RepositoryOptions ConfigureConnectionString(string connectionString)
        {
            var isNullOrEmpty = string.IsNullOrEmpty(connectionString);
            if (isNullOrEmpty)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            ConnectionString = connectionString;

            return this;
        }

        public RepositoryOptions ConfigureRepositories<TInterface, TImplementation>(params string[] namespaces)
        {
            _services.RegisterScoped<TInterface, TImplementation>(namespaces);

            return this;
        }

        public Action<DbContextOptionsBuilder> Builder()
        {
            Validate();

            Action<DbContextOptionsBuilder> builder;

            if (InMemoryEnabled)
            {
                builder = ConfigureInMemory();
            }
            else
            {
                builder = ConfigureConnectionString();
            }


            return builder;
        }

        internal Action<DbContextOptionsBuilder> ConfigureInMemory()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            return options => options
                .UseInMemoryDatabase($"ComponentsOptsolInMemoryDatabase{Guid.NewGuid()}")
                .UseInternalServiceProvider(serviceProvider)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging(LoggingEnabled);
        }

        private Action<DbContextOptionsBuilder> ConfigureConnectionString()
        {
            Action<SqlServerDbContextOptionsBuilder> sqlOptions = null;
            if (!string.IsNullOrEmpty(this.MigrationsAssemblyName))
                sqlOptions = (options) => options.MigrationsAssembly(this.MigrationsAssemblyName);

            return options => options
                .UseSqlServer(ConnectionString, sqlOptions)
                .EnableSensitiveDataLogging(LoggingEnabled);

        }

        private void Validate()
        {
            var incorrectConfiguration = !InMemoryEnabled && string.IsNullOrEmpty(ConnectionString) && string.IsNullOrEmpty(MigrationsAssemblyName);
            if (incorrectConfiguration)
            {
                throw new ConfigurationRepositoryException();
            }
        }
    }

    public static class RepositoryExtensions
    {
        public static IServiceCollection AddContext<TContext>(this IServiceCollection services, Action<RepositoryOptions> options = null)
            where TContext : CoreContext
        {
            var repositoryOptions = new RepositoryOptions(services);
            options?.Invoke(repositoryOptions);

            services.AddDbContext<TContext>(repositoryOptions.Builder());

            services.AddScoped<CoreContext, TContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IReadRepository<,>), typeof(Repository<,>));
            services.AddScoped(typeof(IWriteRepository<,>), typeof(Repository<,>));

            return services;
        }       
    }
}
