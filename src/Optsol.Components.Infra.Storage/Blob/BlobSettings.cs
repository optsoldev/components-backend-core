using Azure.Storage.Blobs.Models;

namespace Optsol.Components.Infra.Storage.Blob;

public class BlobSettings
{
    public string ContentType { get; set; }

    public static implicit operator BlobHttpHeaders(BlobSettings settings)
    {
        if (settings == null) return default;
        
        return new BlobHttpHeaders
        {
            ContentType = settings.ContentType
        };
    }
}