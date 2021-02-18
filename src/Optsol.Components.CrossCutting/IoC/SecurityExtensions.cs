using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Optsol.Components.Infra.Security.Data;
using Optsol.Components.Infra.Security.Services;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class UserStoreOptions
    {
        public Action<DbContextOptionsBuilder> ConfigureDbContext;
    }

    public class ConfigSecurityOptions
    {
        public readonly IServiceCollection ServiceCollection;

        public readonly IIdentityServerBuilder IdentityBuilder;

        public ConfigSecurityOptions(IServiceCollection serviceCollection, IIdentityServerBuilder identityBuilder)
        {
            ServiceCollection = serviceCollection;
            IdentityBuilder = identityBuilder;
        }

        public IServiceCollection AddUserService<TUserService>()
            where TUserService : class, IUserService
        {
            ServiceCollection.AddTransient<IUserService, TUserService>();

            return ServiceCollection;
        }

        public IServiceCollection AddSecurityDataService<TSecurityDataService>()
            where TSecurityDataService : class, ISecurityDataService
        {
            ServiceCollection.AddTransient<ISecurityDataService, TSecurityDataService>();

            return ServiceCollection;
        }

        public IIdentityServerBuilder AddProfileService<TProfileService>()
            where TProfileService: class, IProfileService
        {
            ServiceCollection.AddTransient<IProfileService, TProfileService>();
            return IdentityBuilder.AddProfileService<TProfileService>();
        }
    }

    public static class SecurityStoreExtensions
    {
        public static IServiceCollection AddUserStore(this IServiceCollection services, Action<UserStoreOptions> action)
        {
            var userOptions = new UserStoreOptions();
            action(userOptions);

            services
                .AddDbContext<SecurityDbContext>(userOptions.ConfigureDbContext);

            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        public static IIdentityServerBuilder AddUserStore(this IIdentityServerBuilder builder, Action<UserStoreOptions> action)
        {
            builder.Services.AddUserStore(action);

            return builder;
        }
    }
}