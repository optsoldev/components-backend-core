using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Optsol.Components.Service.Filters;
using Optsol.Components.Service.Middlewares;
using Optsol.Components.Service.Responses;
using Optsol.Components.Service.Transformers;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System.Linq;
using System.Net;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddControllers(option => option.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())))
                .ConfigureNewtonsoftJson();

            services.AddTransient<IResponseFactory, ResponseFactory>();

            services.AddTransient<GlobalExceptionHandlerMiddleware>();

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesProvider = services.BuildServiceProvider();

            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()
                ?? throw new CorsSettingsNullException(servicesProvider.GetRequiredService<ILoggerFactory>());

            services.AddCors(options =>
            {
                foreach (var cors in corsSettings.Policies)
                {
                    options.AddPolicy(cors.Name, _ =>
                    {
                        _.AllowAnyHeader()
                         .AllowAnyMethod()
                         .AllowCredentials()
                         .WithOrigins(cors.Origins.Select(o => o.Value).ToArray());
                    });
                    cors.Validate();
                }
            });

            return services;
        }

        public static IApplicationBuilder UseCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var servicesProvider = app.ApplicationServices;

            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()
                ?? throw new CorsSettingsNullException(servicesProvider.GetRequiredService<ILoggerFactory>());

            app.UseCors(corsSettings.DefaultPolicy);

            return app;
        }

        public static IApplicationBuilder UseException(this IApplicationBuilder app, ILoggerFactory loggerFactory, bool isDevelopment)
        {
            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
                app.UseHsts();
            }
            return app;
        }
    }
}
