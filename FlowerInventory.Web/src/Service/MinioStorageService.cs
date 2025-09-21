using FlowerInventorySystem.FlowerInventory.Web.Configuration;
using FlowerInventorySystem.FlowerInventory.Web.Dto;
using FlowerInventorySystem.FlowerInventory.Web.Model;
using Microsoft.Extensions.Options;
using Minio;
using Minio.ApiEndpoints;
using Minio.DataModel.Args;

namespace FlowerInventorySystem.FlowerInventory.Web.Service;

/* The MiniO Service to connect to the minio instance and perform actions */
public class MinioStorageService(IMinioClient client, IOptions<MinioOptions> opt) : IFileStorage
{
    private readonly MinioOptions _opt = opt.Value;

    /* Upload object to bucket */
    public async Task<string> UploadAsync(IFormFile file, MediaFolder folder, CancellationToken ct = default)
    {
        /* Throw exception on empty file */
        if (file is null || file.Length == 0) throw new ArgumentException("Empty file.");

        /* Construct object path */
        var key = $"{Folder(folder)}/{file.FileName}";

        await using var s = file.OpenReadStream();
        
        /* Create put object for minio using minio api */
        var put = new PutObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(key)
            .WithStreamData(s)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType ?? "application/octet-stream");
        
        /* upload file */
        await client.PutObjectAsync(put, ct);
        return key;
    }

    /* Delete object from bucket */
    public async Task DeleteAsync(string objectName, CancellationToken ct = default)
    {
        await EnsureBucketAsync(ct);
        await client.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(objectName), ct);
    }

    /* Method to fetch objects from certain folder, construct their public url to be shown as gallery */
    public async Task<List<StorageObject>> ListAsync(MediaFolder folder, CancellationToken ct = default)
    {
        var prefix = Folder(folder) + "/";
        var args = new ListObjectsArgs()
            .WithBucket(_opt.Bucket)
            .WithPrefix(prefix)
            .WithRecursive(true);

        var list = new List<StorageObject>();
        var observable = client.ListObjectsAsync(args, ct);

        var tcs = new TaskCompletionSource<bool>();
        using var sub = observable.Subscribe(
            item =>
            {
                list.Add(StorageObject.Builder()
                    .WithKey(item.Key)
                    .WithSize(item.Size)
                    .WithLastModifiedUtc(item.LastModifiedDateTime?.Date.ToUniversalTime())
                    .WithUrl(GetPublicUrl(item.Key))
                    .Build());
            },
            ex => tcs.TrySetException(ex),
            () => tcs.TrySetResult(true)
        );
        await tcs.Task;
        return list;
    }

    /* Helper Method to get path based on enum value */
    private static string Folder(MediaFolder f)
    {
        return f switch
        {
            MediaFolder.Flowers => "flowers",
            MediaFolder.Categories => "categories",
            _ => "default"
        };
    }

    /* Generate a new file name */
    private static string GenFileName(string originalName)
    {
        return $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{Guid.NewGuid():N}{Path.GetExtension(originalName)}";
    }

    /* Method to construct the base url for accessing files */
    public string GetPublicUrl(string key)
    {
        return $"http://{_opt.Endpoint}/{_opt.Bucket}/{Uri.EscapeDataString(key)}";
    }

    /* Method to ensure that bucket exists and if not to create  */
    private async Task EnsureBucketAsync(CancellationToken ct)
    {
        var exists = await client.BucketExistsAsync(new BucketExistsArgs().WithBucket(_opt.Bucket), ct);
        if (!exists)
            await client.MakeBucketAsync(new MakeBucketArgs().WithBucket(_opt.Bucket), ct);
    }

    /* Future Use - Method to create presigned url for downloads */
    public async Task<string> GetPresignedUrlAsync(string objectName, TimeSpan expiry, CancellationToken ct = default)
    {
        await EnsureBucketAsync(ct);
        var args = new PresignedGetObjectArgs()
            .WithBucket(_opt.Bucket)
            .WithObject(objectName)
            .WithExpiry((int)Math.Min(expiry.TotalSeconds, 7 * 24 * 3600));
        return await client.PresignedGetObjectAsync(args);
    }
}