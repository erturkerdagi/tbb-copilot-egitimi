namespace Bank.Api.Services;

/// <summary>
/// Bankacılık günü hesapları. Günlük limitler Türkiye saatine (UTC+3) göre gece yarısında sıfırlanır.
/// Türkiye 2016'dan beri yaz/kış saati uygulamadığı için sabit +3 saat kullanılır.
/// </summary>
public static class BankCalendar
{
    public static readonly TimeSpan TurkeyOffset = TimeSpan.FromHours(3);

    /// <summary>Verilen UTC anının içinde bulunduğu Türkiye gününün başlangıcını UTC olarak döner.</summary>
    public static DateTime StartOfBankDayUtc(DateTime utcNow)
    {
        var turkeyNow = utcNow + TurkeyOffset;
        return DateTime.SpecifyKind(turkeyNow.Date - TurkeyOffset, DateTimeKind.Utc);
    }
}
