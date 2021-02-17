using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        public ConfigSecurityOptions(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
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
    }

    public static class SecurityStoreExtensions
    {
        public static IIdentityServerBuilder AddUserStore(this IIdentityServerBuilder builder, Action<UserStoreOptions> action)
        {
            var userOptions = new UserStoreOptions();
            action(userOptions);

            builder.Services
                .AddDbContext<SecurityDbContext>(userOptions.ConfigureDbContext);

            builder.Services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();

            return builder;
        }
    }
}