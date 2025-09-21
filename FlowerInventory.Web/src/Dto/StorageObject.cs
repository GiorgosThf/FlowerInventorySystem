namespace FlowerInventorySystem.FlowerInventory.Web.Dto;

public class StorageObject(
    string key,
    ulong size,
    DateTime? lastModifiedUtc,
    string? url
)
{
    public string Key => key;
    public ulong Size => size;
    public DateTime? LastModifiedUtc => lastModifiedUtc;
    public string? Url => url;

    public static StorageObjectBuilder Builder()
    {
        return new StorageObjectBuilder();
    }

    public class StorageObjectBuilder
    {
        private string Key { get; set; } = string.Empty;
        private ulong Size { get; set; }
        private DateTime? LastModifiedUtc { get; set; }
        private string? Url { get; set; } = string.Empty;

        public StorageObjectBuilder WithKey(string key)
        {
            Key = key;
            return this;
        }

        public StorageObjectBuilder WithSize(ulong size)
        {
            Size = size;
            return this;
        }

        public StorageObjectBuilder WithLastModifiedUtc(DateTime? lastModifiedUtc)
        {
            LastModifiedUtc = lastModifiedUtc;
            return this;
        }

        public StorageObjectBuilder WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public StorageObject Build()
        {
            return new StorageObject(Key, Size, LastModifiedUtc, Url);
        }
    }
}