﻿using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Shared.Settings;
using RabbitMQ.Client;
using System.Net.Http;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .ConfigureStorage(configuration)
                .ConfigureRedis(configuration)
                .ConfigureElasticSearch(configuration)
                .ConfigureSqlServer(configuration)
                .ConfigureMongoDB(configuration)
                .ConfigureRabbitMQ(configuration)
                .CongigureSecurity(configuration);

            services
                .AddHealthChecksUI(options =>
                {
                    options.SetEvaluationTimeInSeconds(15);
                })
                .AddInMemoryStorage();

            return services;
        }

        private static IHealthChecksBuilder CongigureSecurity(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();

            var hasSecurity = securitySettings is not null;
            if (hasSecurity)
            {
                securitySettings.Validate();

                if (securitySettings.Development.Not())
                {
                    builder
                        .AddCheck("authority", () =>
                        {
                            try
                            {
                                HttpClient client = new HttpClient();
                                var result = client.GetAsync($"{securitySettings.Authority.Endpoint}/health/ping");

                                if (result.IsCompletedSuccessfully)
                                {
                                    return HealthCheckResult.Healthy("Ping is healthy");
                                }

                                return HealthCheckResult.Unhealthy("Ping is unhealthy");
                            }
                            catch
                            {
                                return HealthCheckResult.Unhealthy("Ping is unhealthy");
                            }
                        }, tags: new string[] { "security" });
                }
            }

            return builder;
        }

        private static IHealthChecksBuilder ConfigureStorage(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var storageSettings = configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>();

            var hasAzureStorage = storageSettings is not null;
            if (hasAzureStorage)
            {
                storageSettings.Validate();

                builder
                    .AddAzureBlobStorage(storageSettings.ConnectionString, name: "azure-storage-blob", tags: new string[] { "blob", "file" })
                    .AddAzureQueueStorage(storageSettings.ConnectionString, name: "azure-storage-queue", tags: new string[] { "queue", "event" });
            }

            return builder;
        }

        private static IHealthChecksBuilder ConfigureRedis(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var redisSettings = configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>();

            var hasRedis = redisSettings is not null;
            if (hasRedis)
            {
                redisSettings.Validate();

                builder.AddRedis(redisSettings.ConnectionString, name: "redis", tags: new string[] { "cache", "log" });
            }

            return builder;
        }

        private static IHealthChecksBuilder ConfigureElasticSearch(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var elasticSettings = configuration.GetSection(nameof(ElasticSearchSettings)).Get<ElasticSearchSettings>();

            var hasElastic = elasticSettings is not null;
            if (hasElastic)
            {
                elasticSettings.Validate();

                builder.AddElasticsearch(elasticSettings.Uri, name: "elasticsearch", tags: new string[] { "db", "nosql", "log" });
            }

            return builder;
        }

        private static IHealthChecksBuilder ConfigureSqlServer(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection");

            var hasSQLServer = !string.IsNullOrEmpty(connectionString.Value);
            if (hasSQLServer)
            {
                builder.AddSqlServer(connectionString.Value, name: "sqlserver", tags: new string[] { "db", "database", "sql", "data" });
            }

            return builder;
        }

        private static IHealthChecksBuilder ConfigureMongoDB(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection(nameof(MongoSettings)).Get<MongoSettings>();

            var hasMongoDB = mongoSettings is not null;
            if (hasMongoDB)
            {
                mongoSettings.Validate();

                builder.AddMongoDb(mongoSettings.ConnectionString, name: "mongodb", tags: new string[] { "db", "database", "nosql", "data" });
            }

            return builder;
        }

        private static IHealthChecksBuilder ConfigureRabbitMQ(this IHealthChecksBuilder builder, IConfiguration configuration)
        {
            var rabbitSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

            var hasRabbitMQ = rabbitSettings is not null;
            if (hasRabbitMQ)
            {
                rabbitSettings.Validate();

                var connectionFactory = new ConnectionFactory()
                {
                    HostName = rabbitSettings.HostName,
                    Port = rabbitSettings.Port
                };

                builder.AddRabbitMQ(fac => connectionFactory, name: "rabbitmq", tags: new string[] { "queue", "exchage", "event" });
            }

            return builder;
        }

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseEndpoints(config =>
            {
                config
                    .MapHealthChecks("/health", new HealthCheckOptions
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    })
                    .ConfigureAuthorize(configuration);

                config
                    .MapHealthChecksUI(setup =>
                    {
                        setup.UIPath = "/health-ui";
                        setup.ApiPath = "/health-json";
                    });

                config.MapGet("/health/ping", async context =>
                {
                    await context.Response.WriteAsync("pong");
                });
            });

            return app;
        }

        private static IEndpointConventionBuilder ConfigureAuthorize(this IEndpointConventionBuilder endpoint, IConfiguration configuration)
        {
            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>();
            var hasSecurity = securitySettings is not null;

            if (hasSecurity)
            {
                securitySettings.Validate();

                if (securitySettings.Development)
                {
                    return endpoint;
                }

                endpoint.AllowAnonymous();
            }


            return endpoint;
        }
    }
}
