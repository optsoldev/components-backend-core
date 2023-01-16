using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Optsol.Components.Service.Middlewares;
using Optsol.Components.Service.Responses;
using Optsol.Components.Service.Transformers;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection;

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

        corsSettings.DefaultPolicy?.Validate();
        
        services.AddCors(options =>
        {
            options.AddPolicy(corsSettings!.DefaultPolicy!.Name!, policy =>
            {
                policy.WithOrigins(corsSettings.DefaultPolicy.Origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            if (corsSettings.Policies is null) return;
                
            foreach (var cors in corsSettings.Policies)
            {
                options.AddPolicy(cors.Name!, policy =>
                {
                    policy.WithOrigins(cors.Origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
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

        app.UseCors(corsSettings!.DefaultPolicy!.Name!);

        if (corsSettings.Policies is null) return app;
            
        foreach (var cors in corsSettings.Policies)
        {
            app.UseCors(cors.Name!);
        }
            
        return app;
    }

    public static IApplicationBuilder UseException(this IApplicationBuilder app, bool isDevelopment)
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