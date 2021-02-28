using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Storage.Blob;
using Optsol.Components.Infra.Storage.Queue;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public class StorageOptions
    {
        internal bool BlobEnabled { get; set; } = true;

        internal bool QueueEnabled { get; set; } = true;

        public void DisableBlob()
        {
            BlobEnabled = false;
        }

        public void DisableQueue()
        {
            QueueEnabled = false;
        }
    }

    public static class StorageExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration, Action<StorageOptions> action = null)
        {
            var servicesProvider = services.BuildServiceProvider();

            var storageSettings = configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>()
                ?? throw new StorageSettingsNullException(servicesProvider.GetRequiredService<ILogger<StorageSettingsNullException>>());
            storageSettings.Validate();

            services.AddSingleton(storageSettings);

            var storageOptions = new StorageOptions();
            action?.Invoke(storageOptions);

            if (storageOptions.BlobEnabled)
                services.AddScoped<IBlobStorage, BlobStorage>();

            if (storageOptions.QueueEnabled)
                services.AddScoped<IQueueStorage, QueueStorage>();

            return services;
        }
    }
}
