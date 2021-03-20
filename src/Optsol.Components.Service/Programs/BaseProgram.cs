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
        public static IWebHost BuildWebHost<TStartup>(string[] args)
            where TStartup : class
            => WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configuration) =>
            {
                IHostEnvironment env = context.HostingEnvironment;

                configuration
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);

                Log.Logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration.Build())
                   .WriteTo.Console()
                   .CreateLogger();

            })
            .UseStartup<TStartup>()
            .UseSerilog()
            .UseKestrel()
            .Build();

        public static void Start<TStatup>(string[] args)
            where TStatup : class
        {
            try
            {
                BuildWebHost<TStatup>(args).Run();
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
