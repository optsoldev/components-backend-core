using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Services.Push;
using Optsol.Components.Infra.Firebase.Clients;
using Optsol.Components.Infra.Firebase;
using Optsol.Components.Shared.Settings;
using Refit;
using System;
using System.Threading.Tasks;
using Optsol.Components.Infra.Firebase.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Optsol.Components.Infra.Firebase.Models;

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
            services.AddScoped<IPushService, FirebaseMessagingService>();
            services.AddAutoMapper(typeof(MessageMapper));

            FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, firebaseSettings.FileKeyJson))
            });


            return services;
        }
    }
}
