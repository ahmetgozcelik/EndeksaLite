namespace EndeksaLite.Services;

public class NotificationService
{
    // AnalysisService'deki event tetiklendiğinde bu metot da çalışacak.
    public void HandleAnalysisCompleted(object sender, string message)
    {
        Console.WriteLine($"[BİLDİRİM SİSTEMİ]: {message} (Zaman: {DateTime.Now})");
    }
}