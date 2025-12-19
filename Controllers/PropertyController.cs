using EndeksaLite.Abstractions;
using EndeksaLite.Models;
using EndeksaLite.Services;
using Microsoft.AspNetCore.Mvc;

namespace EndeksaLite.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly AnalysisService _analysisService;
    private readonly NotificationService _notificationService;

    public PropertyController(AnalysisService analysisService, NotificationService notificationService)
    {
        _analysisService = analysisService;
        _notificationService = notificationService;

        _analysisService.OnAnalysisCompleted += _notificationService.HandleAnalysisCompleted;
    }

    [HttpGet("analyze/{source}")]
    public async Task<ActionResult<IEnumerable<AnalysisResult>>> Analyze(string source, [FromKeyedServices(null)] IServiceProvider sp)
    {
        var provider = sp.GetKeyedService<IDataProvider>(source); // url den gelen local ya da external

        if (provider == null) 
            return BadRequest("Kaynak bulunamadı.");

        var rawData = await provider.FetchDataAsync(); // veri çekme
        var analysisResult = _analysisService.Analyze(rawData); // veriyi analiz et

        return Ok(analysisResult);
    }
}