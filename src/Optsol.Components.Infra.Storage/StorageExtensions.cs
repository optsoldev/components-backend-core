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
        private readonly IServiceCollection services;

        public StorageOptions(IServiceCollection services)
        {
            this.services = services;
        }

        public StorageOptions ConfigureBlob<TInterface, TImplementation>()
            where TInterface : IBlobStorage
            where TImplementation : BlobStorageBase, TInterface

        {
            services.AddTransient(typeof(TInterface), typeof(TImplementation));
            return this;
        }

        public StorageOptions ConfigureBlob<TInterface, TImplementation>(StorageSettings settings)
            where TInterface : IBlobStorage
            where TImplementation : BlobStorageBase, TInterface

        {
            services.AddTransient(typeof(TInterface), impl =>
            {
                return Activator.CreateInstance(typeof(TImplementation), new object[] { settings, impl.GetRequiredService<ILoggerFactory>() }) as TImplementation;
            });

            return this;
        }

        public StorageOptions ConfigureQueue<TInterface, TImplementation>()
            where TInterface : IQueueStorage
            where TImplementation : QueueStorageBase
        {
            services.AddTransient(typeof(TInterface), typeof(TImplementation));
            return this;
        }

        public StorageOptions ConfigureQueue<TInterface, TImplementation>(StorageSettings settings)
            where TInterface : IQueueStorage
            where TImplementation : QueueStorageBase
        {
            services.AddTransient(typeof(TInterface), impl =>
            {
                return Activator.CreateInstance(typeof(TImplementation), new object[] { settings, impl.GetRequiredService<ILoggerFactory>() }) as TImplementation;
            });

            return this;
        }
    }

    public static class StorageExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration, Action<StorageOptions> options)
        {
            var servicesProvider = services.BuildServiceProvider();

            var storageSettings = configuration.GetSection(nameof(StorageSettings)).Get<StorageSettings>()
                ?? throw new StorageSettingsNullException(servicesProvider.GetRequiredService<ILoggerFactory>());
            storageSettings.Validate();

            services.AddStorage(storageSettings, options);

            return services;
        }

        public static IServiceCollection AddStorage(this IServiceCollection services, StorageSettings storageSettings, Action<StorageOptions> options)
        {
            services.AddSingleton(storageSettings);

            var storageOptions = new StorageOptions(services);
            options?.Invoke(storageOptions);

            return services;
        }
    }
}
