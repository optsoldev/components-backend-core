using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Optsol.Components.Application.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices<TInterface, TImplementation>(this IServiceCollection services, params string[] namespaces)
        {
            services.RegisterScoped<TInterface, TImplementation>(namespaces);
            services.AddScoped(typeof(IBaseServiceApplication<,,,,>), typeof(BaseServiceApplication<,,,,>));

            return services;
        }

        public static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesProvider = services.BuildServiceProvider();

            var corsSettings = configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>()
                ?? throw new CorsSettingsNullException(servicesProvider.GetRequiredService<ILogger<CorsSettingsNullException>>());

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
                ?? throw new CorsSettingsNullException(servicesProvider.GetRequiredService<ILogger<CorsSettingsNullException>>());

            app.UseCors(corsSettings.DefaultPolicy);

            return app;
        }
    }
}
