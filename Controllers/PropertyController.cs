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
            return BadRequest("Hatalı kaynak! 'local' veya 'external' kullanın.");

        // Analiz servisini bu seçilen provider ile çalıştırıyoruz
        // Not: AnalysisService'i içeride manuel yönetmek yerine constructor'da 
        // IServiceProvider alıp dinamik çözümlemek daha ileri seviye bir tekniktir.
        
        var data = await provider.FetchDataAsync();
        // (Burada analiz mantığını çalıştırdığını varsayıyoruz...)
        return Ok(data);
    }
}