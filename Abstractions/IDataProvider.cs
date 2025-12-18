using EndeksaLite.Models;

namespace EndeksaLite.Abstractions;

public interface IDataProvider
{
    // Geriye 'IEnumerable' yerine 'IReadOnlyCollection' dönmek 
    // "Ben bu veriyi sadece okuyacağım, liste üzerinde ekleme/çıkarma yapmayacağım" demektir. 
    // En temiz (clean) yaklaşım budur.
    Task<IReadOnlyCollection<PriceRecord>> FetchDataAsync();
}