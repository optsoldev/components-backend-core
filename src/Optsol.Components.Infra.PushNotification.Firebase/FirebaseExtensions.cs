﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optsol.Components.Domain.Services.Push;
using System;
using Optsol.Components.Infra.PushNotification.Firebase.Messaging;
using Google.Apis.Auth.OAuth2;
using System.IO;
using Optsol.Components.Infra.PushNotification.Firebase.Mapper;
using FirebaseAdmin;
using Newtonsoft.Json;
using Optsol.Components.Infra.PushNotification.Firebase.Settings;

namespace Microsoft.Extensions.DependencyInjection
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
                Credential = GoogleCredential.FromJson(JsonConvert.SerializeObject(firebaseSettings))
            });

            return services;
        }
    }
}
