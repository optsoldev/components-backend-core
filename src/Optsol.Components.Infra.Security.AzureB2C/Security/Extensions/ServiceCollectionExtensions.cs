using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Optsol.Components.Infra.Security.AzureB2C.Security.Models;
using Optsol.Components.Infra.Security.AzureB2C.Security.Services;
using Optsol.Components.Shared.Settings;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRemoteSecurity(this IServiceCollection services, SecuritySettings securitySettings)
    {
        services.AddTransient<IAuthorityService, AuthorityService>();
        return services;
    }

    public static IConfiguration GetRemoteConfiguration(this IServiceCollection services, SecuritySettings securitySettings)
    {
        var provider = services.BuildServiceProvider();

        OauthClient clientOauth = CreateOauthClient(securitySettings);

        var config = new ConfigurationBuilder();
        config.AddInMemoryObject(clientOauth, "AzureAdB2C");

        services.Configure<OauthClient>("AzureAdB2C", configure =>
        {
            configure = clientOauth;
        });

        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryObject(clientOauth, "AzureAdB2C");
        return configBuilder.Build();
    }

    public static IServiceCollection ConfigureRemoteSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationIsNull = configuration == null;
        if (configurationIsNull)
        {
            return services;
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                configuration?.GetSection("AzureAdB2C").Bind(options);
                options.TokenValidationParameters.NameClaimType = "name";
            }, options =>
            {
                configuration?.GetSection("AzureAdB2C").Bind(options);
            });


        return services;
    }

    public static IServiceCollection ConfigureLocalSecurity(this IServiceCollection services)
    {
        services.AddTransient<IAuthorityService, AuthorityService>();
            
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = LocalSecuritySettings.Issuer,
                    ValidAudience = LocalSecuritySettings.Audience,

                    IssuerSigningKey = LocalSecuritySettings.GetSymmetricSecurityKey()
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = (context) =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var loggerFactory = context.HttpContext.RequestServices
                                .GetRequiredService<ILoggerFactory>();

                            var logger = loggerFactory.CreateLogger("Startup");

                            logger.LogInformation("Token-Expired");
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },

                    OnMessageReceived = _ => Task.FromResult(0)
                };
            });

        return services;
    }

    public static OauthClient CreateOauthClient(SecuritySettings securitySettings)
    {
        return new OauthClient
        {
            Instance = securitySettings.Authority.Instance,
            ClientId = securitySettings.Authority.ClientId,
            Domain = securitySettings.Authority.Domain,
            SignedOutCallbackPath = "/signout/B2C_1_login",
            SignUpSignInPolicyId = "b2c_1_login",
            ResetPasswordPolicyId = "b2c_1_reset",
            EditProfilePolicyId = "b2c_1_edit",
        };
    }
}