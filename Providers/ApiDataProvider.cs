using EndeksaLite.Models;
using EndeksaLite.Abstractions;

namespace EndeksaLite.Providers;

public class ApiDataProvider : IDataProvider
{
    public async Task<IReadOnlyCollection<PriceRecord>> FetchDataAsync()
    {
        await Task.Delay(500);

        return new List<PriceRecord>
        {
            new("Beşiktaş", 8000000, 100, DateTime.Now),
            new("Beşiktaş", 7500000, 95, DateTime.Now),
            new("Kadıköy", 5000000, 90, DateTime.Now),
            new("Kadıköy", 5200000, 92, DateTime.Now),
            new("Sarıyer", 15000000, 150, DateTime.Now)
        }.AsReadOnly();
    }
}