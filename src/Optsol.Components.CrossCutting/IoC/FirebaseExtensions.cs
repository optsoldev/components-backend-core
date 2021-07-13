using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Optsol.Components.Infra.CrossCutting.IoC
{
    public static class FirebaseExtensions
    {
        public static IServiceCollection AddFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            //var firebaseSettings = configuration.GetSection(nameof(FirebaseSettings)).Get<FirebaseSettings>();
            //firebaseSettings.Validate();

            //services.AddSingleton(firebaseSettings);
            //services.AddScoped<IPushService, FirebaseCloudMessaging>();

            //var auth = GetAccessToken(firebaseSettings.Key);

            //services
            //    .AddRefitClient<FirebaseClient>(
            //        new RefitSettings
            //        {
            //            AuthorizationHeaderValueGetter = () => Task.FromResult(auth)
            //        })
            //    .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{firebaseSettings.Url}"));

            //return services;
        }
    }
}
