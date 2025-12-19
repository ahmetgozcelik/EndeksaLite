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

    public List<AnalysisResult> Analyze(IEnumerable<PriceRecord> data)
    {
        if (data == null || !data.Any()) return new List<AnalysisResult>();

        return data.GroupBy(x => x.Neighborhood)
            .Select(group => 
            {
                var avgPrice = group.Average(x => x.Price);
                
                string advice = avgPrice switch
                {
                    > 10000000 => "Lüks Segment - Beklemede Kal",
                    < 6000000 => "Fırsat - Yatırım İçin Uygun",
                    _ => "Normal - Piyasa Değerinde"
                };

                return new AnalysisResult(group.Key, avgPrice, advice, group.Count());
            }).ToList();
    }
}