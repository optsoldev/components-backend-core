using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Optsol.Components.Infra.Data.Interceptors;
using Optsol.Components.Shared.Exceptions;

namespace Microsoft.Extensions.DependencyInjection;

public class RepositoryOptions
{
    private readonly IServiceCollection services;

    internal bool InMemoryEnabled { get; private set; }

    internal bool LoggingEnabled { get; private set; }

    internal IList<IDbCommandInterceptor> Interceptors { get; } = new List<IDbCommandInterceptor>();

    public RepositoryOptions(IServiceCollection services)
    {
        this.services = services;
    }

    /// <summary>
    /// Add TenantCommandInterceptor to EF Interceptors.
    /// </summary>
    /// <returns>Instance of <see cref="RepositoryOptions"/></returns>
    public RepositoryOptions WithTenant()
    {
        var provider = services.BuildServiceProvider();
        var loggedUser = provider.GetService<ITenantProvider>();
        
        Interceptors.Add(new TenantCommandInterceptor<Guid>(loggedUser));

        return this;
    }

    internal string MigrationsAssemblyName { get; private set; }

    internal string ConnectionString { get; private set; }

    public RepositoryOptions EnabledInMemory()
    {
        InMemoryEnabled = true;

        return this;
    }

    public RepositoryOptions EnabledLogging()
    {
        LoggingEnabled = true;

        return this;
    }

    public RepositoryOptions AddInterceptors(params IDbCommandInterceptor[] interceptors)
    {
        foreach (var interceptor in interceptors)
        {
            Interceptors.Add(interceptor);
        }
        
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
        services.RegisterScoped<TInterface, TImplementation>(namespaces);

        return this;
    }

    public Action<DbContextOptionsBuilder> Builder()
    {
        Validate();

        var builder = InMemoryEnabled ? ConfigureInMemory() : ConfigureConnectionString();

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
        if (!string.IsNullOrEmpty(MigrationsAssemblyName))
            sqlOptions = (options) => options.MigrationsAssembly(MigrationsAssemblyName);

        return options => options
            .UseSqlServer(ConnectionString, sqlOptions)
            .EnableSensitiveDataLogging(LoggingEnabled)
            .AddInterceptors(Interceptors);

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