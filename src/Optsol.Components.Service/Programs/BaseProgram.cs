using Microsoft.ApplicationInsights.Extensibility;
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
        protected BaseProgram() { }

        public static IHostBuilder CreateHostBuilder<TStartup>(string[] args)
            where TStartup : class
        {
            return Host
                  .CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<TStartup>();
                  }).ConfigureAppConfiguration((context, configuration) =>
                  {
                      var buildConfiguration = configuration.Build();

                      var logger = new LoggerConfiguration()
                          .ReadFrom.Configuration(buildConfiguration)
                          .WriteTo.Console();

                      logger.ConfigureLogElasticStack(context.HostingEnvironment, buildConfiguration);

                      logger.ConfigureLogApplicationInsights(buildConfiguration);

                      Log.Logger = logger.CreateLogger();

                  })
                  .UseSerilog();
        }

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

    public static class BaseProgramExtensions
    {
        public static LoggerConfiguration ConfigureLogApplicationInsights(this LoggerConfiguration logger, IConfigurationRoot configuration)
        {
            var applicationInsightsSettings = configuration.GetSection(nameof(ApplicationInsightsSettings)).Get<ApplicationInsightsSettings>();
            applicationInsightsSettings?.Validate();

            var applicationInsightsSettingsIsValid = !string.IsNullOrEmpty(applicationInsightsSettings?.InstrumentationKey);
            if (applicationInsightsSettingsIsValid)
            {
                logger.WriteTo.ApplicationInsights(new TelemetryConfiguration(applicationInsightsSettings.InstrumentationKey), TelemetryConverter.Traces);
            }

            return logger;
        }

        public static LoggerConfiguration ConfigureLogElasticStack(this LoggerConfiguration logger, IHostEnvironment env, IConfigurationRoot configuration)
        {
            var elasticSearchSettings = configuration.GetSection(nameof(ElasticSearchSettings)).Get<ElasticSearchSettings>();
            elasticSearchSettings?.Validate();

            var elasticSearchSettingsIsValid = !string.IsNullOrEmpty(elasticSearchSettings?.Uri);
            if (elasticSearchSettingsIsValid)
            {
                var indexNameFormat = $"{elasticSearchSettings.IndexName}-logs-{env.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}";

                logger = logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchSettings.Uri))
                {
                    IndexFormat = indexNameFormat
                });
            }

            return logger;
        }
    }
}
