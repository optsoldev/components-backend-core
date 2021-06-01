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

                      Log.Logger = logger.CreateLogger();

                      logger.ConfigureLogElasticStack(context.HostingEnvironment, buildConfiguration);

                      logger.ConfigureLogApplicationInsights(context.HostingEnvironment, buildConfiguration);
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
        public static LoggerConfiguration ConfigureLogApplicationInsights(this LoggerConfiguration logger, IHostEnvironment hostingEnvironment, IConfigurationRoot configuration)
        {
            if (hostingEnvironment.IsDevelopment())
                return logger;

            var applicationInsightsSettings = configuration.GetSection(nameof(ApplicationInsightsSettings)).Get<ApplicationInsightsSettings>();
            if (applicationInsightsSettings == null)
                return logger;

            Log.Information($"Configure Application Insights Log: {hostingEnvironment.EnvironmentName}");

            applicationInsightsSettings.Validate();

            logger.WriteTo.ApplicationInsights(new TelemetryConfiguration(applicationInsightsSettings.InstrumentationKey), TelemetryConverter.Traces);

            return logger;
        }

        public static LoggerConfiguration ConfigureLogElasticStack(this LoggerConfiguration logger, IHostEnvironment hostingEnvironment, IConfigurationRoot configuration)
        {
            if (hostingEnvironment.IsDevelopment())
                return logger;

            var elasticSearchSettings = configuration.GetSection(nameof(ElasticSearchSettings)).Get<ElasticSearchSettings>();
            if (elasticSearchSettings == null)
                return logger;

            Log.Information($"Configure Elastic Stack Log: {hostingEnvironment.EnvironmentName}");

            elasticSearchSettings.Validate();

            logger = logger.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchSettings.Uri))
            {
                IndexFormat = $"{elasticSearchSettings.IndexName}-logs-{hostingEnvironment.EnvironmentName.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            });

            return logger;
        }
    }
}
