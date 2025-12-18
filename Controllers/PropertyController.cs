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

    [HttpGet("analyze")]
    public async Task<ActionResult<IEnumerable<AnalysisResult>>> Analyze()
    {
        var results = await _analysisService.RunPropertyAnalysisAsync();
        return Ok();
    }
}