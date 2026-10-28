using Bank.Api.Common;

namespace Bank.Api.Services;

public static class InternalTransferErrors
{
    public static readonly Error InvalidAmount =
        new("internal_transfer.invalid_amount", "Virman tutarı sıfırdan büyük olmalıdır.", ErrorType.Validation);

    public static readonly Error SameAccount =
        new("internal_transfer.same_account", "Kaynak ve hedef hesap aynı olamaz.", ErrorType.Validation);

    /// <summary>Hesap yoksa da müşteriye ait değilse de aynı hata döner; başkasının hesabının varlığı ele verilmez.</summary>
    public static readonly Error AccountNotFound =
        new("internal_transfer.account_not_found", "Hesap bulunamadı.", ErrorType.NotFound);

    public static readonly Error CurrencyMismatch =
        new("internal_transfer.currency_mismatch", "Hesapların para birimleri farklı.", ErrorType.BusinessRule);

    public static readonly Error InsufficientBalance =
        new("internal_transfer.insufficient_balance", "Hesap bakiyesi yetersiz.", ErrorType.BusinessRule);
}
