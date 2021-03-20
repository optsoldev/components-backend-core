using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Optsol.Components.Service.Programs
{
    public class BaseProgram
    {
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

                Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration.Build())
                        .WriteTo.Console()
                        .CreateLogger();
            }).UseSerilog();

        public static void Start<TStatup>(IHostBuilder createHostBuilder)
            where TStatup : class
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
