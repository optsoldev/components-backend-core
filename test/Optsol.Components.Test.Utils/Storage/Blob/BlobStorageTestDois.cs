using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Storage.Blob;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Storage.Blob
{
    public class BlobStorageTestDois : BlobStorageBase, IBlobStorageTestDois
    {
        public BlobStorageTestDois(StorageSettings settings, ILoggerFactory logger)
            : base(settings, logger)
        {
        }

        public override string ContainerName { get => "teste-container-dois"; }

        public override PublicAccessType AccessPolicy { get => PublicAccessType.Blob; }
    }
}
