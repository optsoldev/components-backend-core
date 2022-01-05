using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Data;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Storage.Blob
{
    public abstract class BlobStorageBase : IBlobStorage
    {
        private readonly ILogger _logger;
        private readonly StorageSettings _storageSettings;

        private BlobContainerClient _blobContainerClient;

        public BlobStorageBase(StorageSettings settings, ILoggerFactory logger)
        {
            _logger = logger.CreateLogger(nameof(BlobStorageBase));

            _storageSettings = settings ?? throw new StorageSettingsNullException(logger);
            _storageSettings.Validate();
        }

        public abstract string ContainerName { get; }

        public abstract PublicAccessType AccessPolicy { get; }

        public virtual async Task<IEnumerable<Page<BlobItem>>> GetAllAsync()
        {
            StartConnection();

            _logger?.LogInformation($"Executando: {nameof(GetAllAsync)}");

            return await _blobContainerClient.GetBlobsAsync().AsPages().AsyncEnumerableToEnumerable();
        }

        public virtual Task<Response<BlobContentInfo>> UploadAsync(string name, string path)
        {
            StartConnection();

            _logger?.LogInformation($"Executando: {nameof(UploadAsync)}({name}, {path}) Retorno: Task");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.UploadAsync(path, overwrite: true);
        }

        public virtual Task<Response<BlobContentInfo>> UploadAsync(string name, Stream stream)
        {
            StartConnection();

            _logger?.LogInformation($"Executando: {nameof(UploadAsync)}({name}, stream) Retorno: Task");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.UploadAsync(stream, overwrite: true);
        }

        public virtual Task DeleteAsync(string name)
        {
            StartConnection();


            _logger?.LogInformation($"Executando: {nameof(DeleteAsync)}({name}) Retorno: Task");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.DeleteIfExistsAsync();
        }

        public virtual Task<Response<BlobDownloadInfo>> DownloadAsync(string name)
        {
            StartConnection();

            _logger?.LogInformation($"Executando: {nameof(DeleteAsync)}({name}) Retorno: Task<Response<BlobDownloadInfo>>");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.DownloadAsync();
        }

        public virtual Task<Uri> GetUriAsync(string name)
        {
            StartConnection();

            string path = $"{_blobContainerClient.Uri}/{name}";

            var uri = new Uri(path);

            return Task.FromResult(uri);
        }

        public virtual Task<Response<bool>> ContainerExistsAsync()
        {
            StartConnection();

            return _blobContainerClient.ExistsAsync();
        }

        private void StartConnection()
        {
            var containerClientCreated = _blobContainerClient != null;
            if (containerClientCreated)
            {
                return;
            }

            var blobContainerClient = new BlobContainerClient(_storageSettings.ConnectionString, ContainerName);

            blobContainerClient.CreateIfNotExists();

            blobContainerClient.SetAccessPolicy(AccessPolicy);

            _blobContainerClient = blobContainerClient;
        }
    }
}
