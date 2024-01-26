namespace Ase.Doc.Demo.Configuration;

public class AppConfig
{
    public AppConfig(IConfiguration config)
    {
        DocumentAi = new AzureDocumentAiConfig(
            config["Azure:AI:Document:Endpoint"],
            config["Azure:AI:Document:Key"]);

        Storage = new StorageConfig(
            config["Azure:Storage:AccountName"],
            config["Azure:Storage:AccountKey"]);
    }
    public AzureDocumentAiConfig DocumentAi { get; private set; }

    public StorageConfig Storage { get; private set; }
}