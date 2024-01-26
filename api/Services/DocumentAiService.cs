using Ase.Doc.Demo.Configuration;
using Ase.Doc.Demo.Model;
using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace Ase.Doc.Demo.Services;

public class DocumentAiService
{
    private readonly AzureDocumentAiConfig config;
    private readonly ILogger<DocumentAiService> logger;

    public DocumentAiService(AppConfig config, ILogger<DocumentAiService> logger)
    {
        this.config = config.DocumentAi;
        this.logger = logger;
    }

    //bentley-model
    public async Task<object> GetResultsAsync(string model, string uri)
    {
        var aiResults = await GetAiResultsAsync(model, uri);

        var document = aiResults.Documents.SingleOrDefault(x => x.DocumentType == model);
        var results = new UploadResults
        {
            Values = new List<UploadResultValue>()
        };

        logger.LogInformation("Number of keys: " + document.Fields.Keys.Count().ToString());

        foreach (var field in document.Fields.Keys)
        {
            var content = document.Fields[field].Content;

            if (field == "DealerCode")
                results.DealerCode = content;
            else if (field == "DealerName")
                results.DealerName = content;
            else if (field == "From")
                results.From = DateTime.Parse(content);
            else if (field == "To")
                results.To = DateTime.Parse(content);
            else
                results.Values.Append(new UploadResultValue
                {
                    Code = field,
                    Value = content,
                    Confidence = document.Fields[field].Confidence
                });
        }

        return document.Fields;
        //return results;
    }

    private async Task<AnalyzeResult> GetAiResultsAsync(string model, string uri)
    {
        var client = new DocumentAnalysisClient(
            new Uri(config.Endpoint),
            new AzureKeyCredential(config.Key));

        var fileUri = new Uri("https://asesingapore.blob.core.windows.net/aistorage/test.pdf?sv=2021-10-04&st=2024-01-24T09%3A13%3A31Z&se=2025-01-25T09%3A13%3A00Z&sr=b&sp=r&sig=UJlUojI2NorSg%2F12T6HpunvZQ%2Fu1imgT%2FiGY%2FC5TEKM%3D");
        var operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, model, fileUri);

        return operation.Value;
    }
}
