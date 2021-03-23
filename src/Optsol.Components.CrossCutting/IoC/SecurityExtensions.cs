using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SecurityStoreExtensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = services.BuildServiceProvider();

            var securitySettings = configuration.GetSection(nameof(SecuritySettings)).Get<SecuritySettings>()
                ?? throw new SecuritySettingNullException(provider.GetRequiredService<ILoggerFactory>());

            securitySettings.Validate();

            services.AddSingleton(securitySettings);

            var azureB2C = $"{nameof(SecuritySettings)}:{nameof(SecuritySettings.AzureB2C)}";

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(options =>
                {
                    configuration.GetSection(azureB2C).Bind(options);
                    options.TokenValidationParameters.NameClaimType = "name";
                }, options => 
                { 
                    configuration.GetSection(azureB2C).Bind(options); 
                });

            return services;
        }

        public static IApplicationBuilder UseSecurity(this IApplicationBuilder app, bool development)
        {
            if (development)
            {
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseAuthentication()
               .UseAuthorization();

            return app;
        }
    }
}