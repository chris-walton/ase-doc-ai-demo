namespace Ase.Doc.Demo.Configuration;

public class AzureDocumentAiConfig
{
    public string Endpoint { get; set; }
    public string Key { get; set; }

    //Azure:AI:Document
    public AzureDocumentAiConfig(string endpoint, string key)
    {
        Key = key;
        Endpoint = endpoint;
    }
}