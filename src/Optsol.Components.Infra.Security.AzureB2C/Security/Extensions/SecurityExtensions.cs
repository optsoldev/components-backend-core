using System.Security.Principal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Securities;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Security.AzureB2C.Security.Services;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class SecurityStoreExtensions
    {
        public static IServiceCollection AddLoggedUser(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            
            services.AddScoped<ILoggedUser<Guid>, LoggedUser>();

            return services;
        }
        
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                ?? throw new SecuritySettingNullException(provider.GetRequiredService<ILoggerFactory>());

            securitySettings.Validate();

            services.AddSingleton(securitySettings);
 
            if (securitySettings.Development)
            {
                services.ConfigureLocalSecurity();
            }
            else
            {
                services.AddRemoteSecurity(securitySettings);
                var configInMemory = services.GetRemoteConfiguration(securitySettings);
                services.ConfigureRemoteSecurity(configInMemory);
            }

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, IConfiguration configuration)
        {
            var loggerFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(UseSecurity));

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                ?? throw new SecuritySettingNullException(loggerFactory);
            securitySettings.Validate();

            app.UseAuthentication();
            app.UseAuthorization();

            if (securitySettings.Development)
            {
                logger?.LogInformation("Configurando Segurança Local (IsDevelopment: true)");

                app.UseRemoteEndpoint(securitySettings);
            }
            else
            {
                logger?.LogInformation("Configurando Segurança Remota (IsDevelopment: false)");
            }

            return app;
        }
        
        public static bool IsAuthenticated(this IPrincipal? principal)
        {
            return principal is { Identity.IsAuthenticated: true };
        }
    }
}