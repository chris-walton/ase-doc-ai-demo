namespace Ase.Doc.Demo.Configuration;

public class StorageConfig
{
    public StorageConfig(string accountName, string accountKey)
    {
        AccountName = accountName;
        AccountKey = accountKey;
    }

    public string AccountName { get; private set; }
    public string AccountKey { get; private set; }
}