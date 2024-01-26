using Ase.Doc.Demo.Configuration;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;

namespace Ase.Doc.Demo.Services;

public class Storage
{
    private readonly AppConfig config;
    private readonly BlobServiceClient service; //TODO Remove use of
    private readonly StorageSharedKeyCredential credentials;

    public Storage(AppConfig config)
    {
        this.config = config;

        credentials = new StorageSharedKeyCredential(config.Storage.AccountName, config.Storage.AccountKey);
        service = new BlobServiceClient(
        new Uri($"https://{config.Storage.AccountName}.blob.core.windows.net"),
             credentials);
    }

    private string AccountName => config.Storage.AccountName;

    public async Task<byte[]> GetFileAsBytesAsync(string containerName, string fileName)
    {
        var container = await GetContainerAsync(containerName);
        var blob = container.GetBlobClient(fileName);

        if (!await blob.ExistsAsync()) return null;

        BlobDownloadStreamingResult results = await blob.DownloadStreamingAsync();

        using (var stream = new MemoryStream())
        {
            results.Content.CopyTo(stream);

            return stream.ToArray();
        }
    }

    public async Task<BlobClient> SaveFileAsync(string containerName, string fileName, byte[] data, bool snapshot)
    {
        var container = await GetContainerAsync(containerName);
        var blob = container.GetBlobClient(fileName);

        if (snapshot && await blob.ExistsAsync()) await blob.CreateSnapshotAsync();

        await blob.UploadAsync(new BinaryData(data));

        return blob;
    }

    public async Task SaveFileAsync(string containerName, string fileName, string data, bool snapshot)
    {
        var container = await GetContainerAsync(containerName);
        var blob = container.GetBlobClient(fileName);

        if (snapshot && await blob.ExistsAsync()) await blob.CreateSnapshotAsync();

        await blob.UploadAsync(new BinaryData(data));
    }

    private async Task<BlobContainerClient> GetContainerAsync(string containerName)
    {
        var container = service.GetBlobContainerClient(containerName.ToLower());

        await container.CreateIfNotExistsAsync();

        return container;
    }

    public Uri CreateServiceSASBlob(string containerName, string fileName)
    {
        //build the blob container url
        var blobUrl = string.Format("https://{0}.blob.core.windows.net/{1}/{2}", AccountName, containerName, fileName);

        //directly build BlobContainerClient, then pass it to GetServiceSasUriForContainer() method
        var blob = new BlobClient(new Uri(blobUrl), credentials);

        // Check if BlobContainerClient object has been authorized with Shared Key
        if (!blob.CanGenerateSasUri) return null;

        // Create a SAS token that's valid for 10 minutes
        var sasBuilder = new BlobSasBuilder
        {
            Resource = "b",
            BlobName = blob.Name,
            BlobContainerName = blob.GetParentBlobContainerClient().Name,
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(10)
        };
        sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

        return blob.GenerateSasUri(sasBuilder);
    }
}
