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
    public async Task<object> GetResultsAsync(string model, Uri uri)
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
    }

    private async Task<AnalyzeResult> GetAiResultsAsync(string model, Uri uri)
    {
        var client = new DocumentAnalysisClient(
            new Uri(config.Endpoint),
            new AzureKeyCredential(config.Key));

        var operation = await client.AnalyzeDocumentFromUriAsync(WaitUntil.Completed, model, uri);

        return operation.Value;
    }
}
