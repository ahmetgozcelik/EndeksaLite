using EndeksaLite.Abstractions;
using EndeksaLite.Models;

namespace EndeksaLite.Services;

public class AnalysisService
{
    private readonly IDataProvider _dataProvider;
    
    public delegate void AnalysisCompleteHandler(object sender, string message);// haber hangi parametreleri taşıyacak?
    public event AnalysisCompleteHandler? OnAnalysisCompleted;// haberin kendisi

    public AnalysisService(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public async Task<List<AnalysisResult>> RunPropertyAnalysisAsync()
    {
        var data = await _dataProvider.FetchDataAsync(); // veri interface üzerinden alındı.
        if (!data.Any()) return new List<AnalysisResult>(); // Any()->var mı yok mu olayı, bool döner. performanslıdır. sebebi listeyi tarar.

        var results = data.GroupBy(x => x.Neighborhood)
            .Select(group =>
            {
                var avgPrice = group.Average(x => x.Price);
                
                // basit yatırım tavsiyesi mantığı
                string advice = avgPrice switch
                {
                    > 10000000 => "Lüks Segment - Beklemede Kal",
                    < 6000000 => "Fırsat - Yatırım İçin Uygun",
                    _ => "Normal - Piyasa Değerinde"
                };

                return new AnalysisResult(
                    group.Key,
                    avgPrice,
                    advice,
                    group.Count()
                );
            }).ToList();
        
        OnAnalysisCompleted?.Invoke(this, $"Analiz başarıyla tamamlandı. {results.Count} bölge incelendi.");

        return results;
    }
}