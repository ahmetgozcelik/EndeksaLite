using System.Text.Json;
using EndeksaLite.Abstractions;
using EndeksaLite.Models;

using EndeksaLite.Providers;

public class HttpDataProvider : IDataProvider, IDisposable
{
    public readonly IHttpClientFactory _httpClientFactory;

    public HttpDataProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyCollection<PriceRecord>> FetchDataAsync()
    {
        var client = _httpClientFactory.CreateClient();
        
        try 
        {
            var response = await client.GetAsync("https://mocki.io/v1/8d002aa7-e547-473a-b0ac-dfc9c735b2ef");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            
            // Bu ayar hayati önem taşıyor:
            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true // Küçük/büyük harf farkını görmezden gel
            };
            
            var data = JsonSerializer.Deserialize<List<PriceRecord>>(json, options);

            return data?.AsReadOnly() ?? new List<PriceRecord>().AsReadOnly();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[HATA]: {ex.Message}");
            return new List<PriceRecord>().AsReadOnly();
        }
    }

    public void Dispose()
    {
        // Temizlik işlemleri burada yapılır. 
        // HttpClientFactory kullanıldığı için .NET bunu yönetir ama 
        // bu yapıyı kurmak senin temizlik disiplinini gösterir.
        Console.WriteLine("[KAYNAK YÖNETİMİ]: HttpDataProvider kapatıldı ve kaynaklar serbest bırakıldı.");
    }
}