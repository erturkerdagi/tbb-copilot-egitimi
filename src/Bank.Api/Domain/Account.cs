namespace Bank.Api.Domain;

/// <summary>
/// Vadesiz hesap. Bakiye yalnızca Deposit ve Withdraw metotlarıyla değişir.
/// </summary>
public class Account
{
    public int Id { get; set; }

    public string Iban { get; set; } = string.Empty;

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    /// <summary>Para birimi kodu (ISO 4217), örn. "TRY".</summary>
    public string Currency { get; set; } = "TRY";

    public decimal Balance { get; private set; }

    /// <summary>Bu hesaptan bir günde gönderilebilecek en yüksek toplam tutar.</summary>
    public decimal DailyTransferLimit { get; set; }

    public Account()
    {
    }

    public Account(int id, string iban, int customerId, string currency, decimal openingBalance, decimal dailyTransferLimit)
    {
        Id = id;
        Iban = iban;
        CustomerId = customerId;
        Currency = currency;
        Balance = openingBalance;
        DailyTransferLimit = dailyTransferLimit;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Yatırılan tutar sıfırdan büyük olmalıdır.");
        }

        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Çekilen tutar sıfırdan büyük olmalıdır.");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException("Yetersiz bakiye.");
        }

        Balance -= amount;
    }
}
