namespace Bank.Api.Services;

/// <summary>
/// Günlük transfer limiti kontrolü.
/// İş kuralı: Müşteri bir gün içinde, limit dahil olmak üzere, limite kadar transfer yapabilir.
/// </summary>
public class TransferLimitChecker
{
    /// <param name="amount">Gönderilmek istenen tutar.</param>
    /// <param name="alreadySentToday">Bugün bu hesaptan daha önce gönderilen toplam tutar.</param>
    /// <param name="dailyLimit">Hesabın günlük transfer limiti.</param>
    /// <returns>Transfer limit içindeyse true.</returns>
    public bool IsWithinDailyLimit(decimal amount, decimal alreadySentToday, decimal dailyLimit)
    {
        return alreadySentToday + amount < dailyLimit;
    }
}
