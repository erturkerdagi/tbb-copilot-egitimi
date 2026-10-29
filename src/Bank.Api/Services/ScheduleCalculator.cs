namespace Bank.Api.Services;

/// <summary>
/// Düzenli ödeme talimatlarının gerçekleşme tarihini hesaplar (spec 002, FR-005 ve FR-006).
/// </summary>
public static class ScheduleCalculator
{
    /// <summary>
    /// Verilen dönem (yıl/ay) için gerçekleşme tarihi.
    /// 1) Ay, talimat gününden kısaysa ayın son günü kullanılır.
    /// 2) Tarih hafta sonuna denk gelirse sonraki pazartesiye kayar (bir sonraki aya taşabilir).
    /// Resmi tatiller bu sürümün kapsamı dışındadır.
    /// </summary>
    public static DateOnly ExecutionDateFor(int dayOfMonth, int year, int month)
    {
        if (dayOfMonth is < 1 or > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(dayOfMonth), "Gün 1 ile 31 arasında olmalıdır.");
        }

        var day = Math.Min(dayOfMonth, DateTime.DaysInMonth(year, month));
        var date = new DateOnly(year, month, day);

        while (date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            date = date.AddDays(1);
        }

        return date;
    }
}
