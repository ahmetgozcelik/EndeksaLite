namespace EndeksaLite.Models;

// record -> nesne oluşturulduktan sonra değiştirilemez. Veri güvenliği için iyi bir yöntem.
public record PriceRecord(
    string Neighborhood,
    decimal Price,
    int SquareMeter,
    DateTime ListingDate
)
{
    public decimal UnitPrice = Price / SquareMeter;
}

public record AnalysisResult(
    string Neighborhood,
    decimal AveragePrice,
    string InvestmentAdvice, // fırsat - normal - pahalı
    int SampleCount
);