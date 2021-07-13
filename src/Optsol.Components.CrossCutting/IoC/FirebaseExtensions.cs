using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.Firebase.Clients;
using Optsol.Components.Infra.Firebase.Services;
using Optsol.Components.Shared.Settings;
using Refit;
using System;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.CrossCutting.IoC
{
    public static class FirebaseExtensions
    {
        public static IServiceCollection AddFirebase(this IServiceCollection services, IConfiguration configuration)
        {

            var firebaseSettings = new FirebaseSettings();
            configuration.GetSection(nameof(FirebaseSettings)).Bind(firebaseSettings);
            firebaseSettings.Validate();

            services.AddSingleton(firebaseSettings);
            services.AddScoped<IPushService, FirebaseCloudMessagingServices>();

            var secretKeyFirebase = $"={firebaseSettings.Key}";

            services
                .AddRefitClient<FirebaseClient>(
                    new RefitSettings
                    {
                        AuthorizationHeaderValueGetter = () => Task.FromResult(secretKeyFirebase)
                    })
                .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{firebaseSettings.Url}"));

            return services;
        }
    }
}
