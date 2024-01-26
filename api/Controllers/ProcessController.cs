using Ase.Doc.Demo.Services;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Ase.Doc.Demo.Controllers;

[ApiController]
[EnableCors("MyPolicy")]
[Route("api/[controller]")]
public class ProcessController : ControllerBase
{
    private readonly TelemetryClient telemetry;
    private readonly ILogger<ProcessController> logger;
    private readonly Storage storage;
    private readonly DocumentAiService documentAiService;

    public ProcessController(TelemetryClient telemetry, ILogger<ProcessController> logger, Storage storage, DocumentAiService documentAiService)
    {
        this.logger = logger;
        this.storage = storage;
        this.telemetry = telemetry;
        this.documentAiService = documentAiService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    //[Authorize]
    [HttpPost("{model}/{fileName}")]
    public async Task<IActionResult> PostAi(string model, string fileName)
    {
        try
        {
            Request.EnableBuffering();
            Request.Body.Position = 0;

            using (var stream = new MemoryStream())
            {
                await Request.Body.CopyToAsync(stream);

                fileName = $"{DateTime.Now:yyyyMMddHHmmss}-${model}-${fileName}";

                var blob = await storage.SaveFileAsync("aistorage", fileName, stream.ToArray(), true);

                return Ok(await documentAiService.GetResultsAsync(model, blob.Uri.ToString()));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing AI file");
            telemetry.TrackException(ex);
            return new StatusCodeResult(500);
        }
    }
}

