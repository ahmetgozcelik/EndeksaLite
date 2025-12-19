using EndeksaLite.Abstractions;
using EndeksaLite.Providers;
using EndeksaLite.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Servis Kayıt Bölümü (Dependency Injection) ---

// ÖNEMLİ: Controller yapısını kullanabilmek için bu satır ŞART
builder.Services.AddControllers(); 

builder.Services.AddOpenApi();

builder.Services.AddHttpClient();

// Proje Servislerin
builder.Services.AddScoped<IDataProvider, ApiDataProvider>();
builder.Services.AddScoped<AnalysisService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddKeyedScoped<IDataProvider, ApiDataProvider>("local");
builder.Services.AddKeyedScoped<IDataProvider, HttpDataProvider>("external");

var app = builder.Build();

// --- 2. HTTP Request Pipeline (Middleware) ---

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}

// Lokal geliştirmede bazen sorun çıkardığı için şimdilik opsiyonel
// app.UseHttpsRedirection();

// --- 3. Endpoint ve Controller Tanımlamaları ---

// ÖNEMLİ: Yazdığın PropertyController'ı Scalar/Swagger'a bağlayan sihirli satır:
app.MapControllers();

// Örnek WeatherForecast (Minimal API olarak kalabilir, zararı yok)
app.MapGet("/weatherforecast", () =>
{
    var summaries = new[] { "Chilly", "Cool", "Mild", "Warm", "Hot" };
    return Enumerable.Range(1, 5).Select(index =>
        new {
            Date = DateTime.Now.AddDays(index),
            Temp = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        });
})
.WithName("GetWeatherForecast");

app.Run();