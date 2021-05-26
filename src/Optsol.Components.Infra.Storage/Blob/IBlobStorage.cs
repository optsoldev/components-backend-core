using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Optsol.Components.Infra.Storage.Blob
{
    public interface IBlobStorage
    {
        string ContainerName { get; }

        PublicAccessType AccessPolicy { get; }

        Task<IEnumerable<Page<BlobItem>>> GetAllAsync();

        Task<Response<BlobContentInfo>> UploadAsync(string name, Stream stream);

        Task<Response<BlobContentInfo>> UploadAsync(string name, string path);

        Task DeleteAsync(string name);

        Task<Response<BlobDownloadInfo>> DownloadAsync(string name);
        
        Task<Uri> GetUriAsync(string name);

        Task<Response<bool>> ContainerExistsAsync();
    }
}
