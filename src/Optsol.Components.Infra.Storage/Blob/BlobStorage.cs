using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Optsol.Components.Shared.Exceptions;
using Optsol.Components.Shared.Extensions;
using Optsol.Components.Shared.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Storage.Blob
{
    public class BlobStorage : IBlobStorage
    {
        private readonly ILogger _logger;

        private readonly StorageSettings _storageSettings;

        private readonly BlobContainerClient _blobContainerClient;

        public BlobStorage(StorageSettings settings, ILogger<BlobStorage> logger, ILogger<StorageSettingsNullException> loggerException)
        {
            _storageSettings = settings ?? throw new StorageSettingsNullException(loggerException);
            _storageSettings.Validate();

            _logger = logger;

            var blobContainerClient = new BlobContainerClient(settings.ConnectionString, _storageSettings.Blob.ContainerName);

            blobContainerClient.CreateIfNotExists();

            _blobContainerClient = blobContainerClient;
        }

        public virtual async Task<IEnumerable<Page<BlobItem>>> GetAllAsync()
        {
            _logger.LogInformation($"Executando: {nameof(GetAllAsync)}");

            return await _blobContainerClient.GetBlobsAsync().AsPages().AsyncEnumerableToEnumerable();
        }

        public virtual Task UploadAsync(string name, string path)
        {
            _logger.LogInformation($"Executando: {nameof(UploadAsync)}({name}, {path}) Retorno: Task");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.UploadAsync(path, overwrite: true);
        }

        public virtual Task UploadAsync(string name, Stream stream)
        {
            _logger.LogInformation($"Executando: {nameof(UploadAsync)}({name}, stream) Retorno: Task");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.UploadAsync(stream, overwrite: true);
        }

        public virtual Task DeleteAsync(string name)
        {
            _logger.LogInformation($"Executando: {nameof(DeleteAsync)}({name}) Retorno: Task");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.DeleteIfExistsAsync();
        }

        public virtual Task<Response<BlobDownloadInfo>> DowloadAsync(string name)
        {
            _logger.LogInformation($"Executando: {nameof(DeleteAsync)}({name}) Retorno: Task<Response<BlobDownloadInfo>>");

            var blob = _blobContainerClient.GetBlobClient(name);

            return blob.DownloadAsync();
        }

        public virtual Task<Response<bool>> ContainerExistsAsync()
        {
            return _blobContainerClient.ExistsAsync();
        }
    }
}
