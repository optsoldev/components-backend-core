using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Optsol.Components.Shared.Exceptions;

namespace Optsol.Components.Infra.IoC
{
    public class ContextOptionsBuilder
    {
        public bool InMemory { get; private set; }
        public string ConnectionString { get; private set; }

        public string MigrationsAssemblyName { get; private set; }

        public ContextOptionsBuilder()
        {
            InMemory = true;
            ConnectionString = "InMemory";
        }

        public ContextOptionsBuilder(string connectionString)
        {
            this.ConnectionString = !string.IsNullOrEmpty(connectionString) 
                ? connectionString : throw new ConnectionStringNullException();

            InMemory = false;
        }

        public ContextOptionsBuilder(string connectionString, string migrationsAssembly)
        : this(connectionString)
        {
            this.MigrationsAssemblyName = migrationsAssembly;
        }

        public Action<DbContextOptionsBuilder> Builder()
        {
            Action<DbContextOptionsBuilder> builder;
            if(InMemory)
                builder = BuilderInMemory();
            else
                builder = BuilderConnectionString();

            return builder;
        }

        public Action<DbContextOptionsBuilder> BuilderInMemory()
        {
            return options => options.UseInMemoryDatabase("ComponentsOptsolInMemoryDatabase");
        }

        public Action<DbContextOptionsBuilder> BuilderConnectionString()
        {
            Action<SqlServerDbContextOptionsBuilder> sqlOptions = null;
            if (!string.IsNullOrEmpty(this.MigrationsAssemblyName))
                sqlOptions = (options) => options.MigrationsAssembly(this.MigrationsAssemblyName);

            return options => options.UseSqlServer(ConnectionString, sqlOptions);
        }
    }
}
