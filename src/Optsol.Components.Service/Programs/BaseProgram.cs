using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Optsol.Components.Shared.Settings;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace Optsol.Components.Service.Programs
{
    public class BaseProgram
    {
        protected BaseProgram()
        {

        }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args)
            where TStartup : class
            => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<TStartup>();
            }).ConfigureAppConfiguration((context, configuration) =>
            {
                IHostEnvironment env = context.HostingEnvironment;

                configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                var buildConfiguration = configuration.Build();
                                
                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(buildConfiguration)
                    .WriteTo.Console();

                var elasticSearchSettings = buildConfiguration.GetSection(nameof(ElasticSearchSettings)).Get<ElasticSearchSettings>();
                var elasticSearchSettingsIsValid = elasticSearchSettings != null && !string.IsNullOrEmpty(elasticSearchSettings.Uri);
                if (elasticSearchSettingsIsValid)
                {
                    var indexNameFormat = $"{elasticSearchSettings.IndexName}-logs-{env.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";

                    logger = logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchSettings.Uri))
                    {
                        IndexFormat = indexNameFormat
                    });
                }

                Log.Logger = logger.CreateLogger();

            }).UseSerilog();

        public static void Start(IHostBuilder createHostBuilder)
        {
            try
            {
                createHostBuilder.Build().Run();

                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
