using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;
using Optsol.Components.Infra.Storage.Blob;
using Optsol.Components.Shared.Settings;

namespace Optsol.Components.Test.Utils.Storage.Blob
{
    public class BlobStorageTest : BlobStorageBase, IBlobStorageTest
    {
        public BlobStorageTest(StorageSettings settings, ILoggerFactory logger)
            : base(settings, logger)
        {
        }

        public override string ContainerName { get => "teste-container"; }

        public override PublicAccessType AccessPolicy { get => PublicAccessType.Blob; }
    }
}
