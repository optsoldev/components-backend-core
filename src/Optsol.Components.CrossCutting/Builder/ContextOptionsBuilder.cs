using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Optsol.Components.Shared.Exceptions;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ContextOptionsBuilder
    {
        public bool InMemory { get; private set; }

        public bool EnableLogging { get; private set; }

        public string ConnectionString { get; private set; }

        public string MigrationsAssemblyName { get; private set; }


        public ContextOptionsBuilder()
        {
            InMemory = true;
            EnableLogging = true;
            ConnectionString = "InMemory";
        }

        public ContextOptionsBuilder(string connectionString)
        {
            ConnectionString = !string.IsNullOrEmpty(connectionString)
                ? connectionString : throw new ConnectionStringNullException();

            InMemory = false;
        }

        public ContextOptionsBuilder(string connectionString, string migrationsAssembly)
        : this(connectionString)
        {
            this.MigrationsAssemblyName = migrationsAssembly;
        }

        public ContextOptionsBuilder(string connectionString, string migrationsAssembly, bool enableLogging)
        : this(connectionString, migrationsAssembly)
        {
            EnableLogging = enableLogging;
        }

        public Action<DbContextOptionsBuilder> Builder()
        {
            Action<DbContextOptionsBuilder> builder;
            if (InMemory)
                builder = BuilderInMemory();
            else
                builder = BuilderConnectionString();
                     
            return builder;
        }

        private Action<DbContextOptionsBuilder> BuilderInMemory()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            return options => options
                .UseInMemoryDatabase($"ComponentsOptsolInMemoryDatabase{Guid.NewGuid()}")
                .UseInternalServiceProvider(serviceProvider)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                .EnableSensitiveDataLogging(EnableLogging);
        }

        private Action<DbContextOptionsBuilder> BuilderConnectionString()
        {
            Action<SqlServerDbContextOptionsBuilder> sqlOptions = null;
            if (!string.IsNullOrEmpty(this.MigrationsAssemblyName))
                sqlOptions = (options) => options.MigrationsAssembly(this.MigrationsAssemblyName);

            return options => options
                .UseSqlServer(ConnectionString, sqlOptions)
                .EnableSensitiveDataLogging(EnableLogging);

        }
    }
}
