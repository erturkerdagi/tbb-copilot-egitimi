using Bank.Api.Common;

namespace Bank.Api.Services;

public static class TransferErrors
{
    public static readonly Error InvalidAmount =
        new("transfer.invalid_amount", "Transfer tutarı sıfırdan büyük olmalıdır.", ErrorType.Validation);

    public static readonly Error SameAccount =
        new("transfer.same_account", "Gönderen ve alıcı hesap aynı olamaz.", ErrorType.Validation);

    public static readonly Error SourceAccountNotFound =
        new("transfer.source_not_found", "Gönderen hesap bulunamadı.", ErrorType.NotFound);

    public static readonly Error TargetAccountNotFound =
        new("transfer.target_not_found", "Alıcı hesap bulunamadı.", ErrorType.NotFound);

    public static readonly Error CurrencyMismatch =
        new("transfer.currency_mismatch", "Hesapların para birimleri farklı.", ErrorType.BusinessRule);

    public static readonly Error InsufficientBalance =
        new("transfer.insufficient_balance", "Hesap bakiyesi yetersiz.", ErrorType.BusinessRule);

    public static readonly Error DailyLimitExceeded =
        new("transfer.daily_limit_exceeded", "Günlük transfer limiti aşıldı.", ErrorType.BusinessRule);
}

public static class AccountErrors
{
    public static readonly Error NotFound =
        new("account.not_found", "Hesap bulunamadı.", ErrorType.NotFound);
}
