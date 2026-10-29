using Bank.Api.Common;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IPaymentInstructionRepository
{
    Task AddAsync(PaymentInstruction instruction, CancellationToken ct);

    Task<IReadOnlyList<PaymentInstruction>> GetByCustomerAsync(int customerId, CancellationToken ct);

    Task<IReadOnlyList<PaymentInstruction>> GetActiveAsync(CancellationToken ct);

    /// <summary>Talimat bu dönem için daha önce çalıştırıldı mı (başarılı veya başarısız)?</summary>
    Task<bool> HasRunAsync(int instructionId, int periodYear, int periodMonth, CancellationToken ct);

    Task AddRunAsync(PaymentInstructionRun run, CancellationToken ct);
}

public static class PaymentInstructionErrors
{
    public static readonly Error InvalidAmount =
        new("payment_instruction.invalid_amount", "Talimat tutarı sıfırdan büyük olmalıdır.", ErrorType.Validation);

    public static readonly Error InvalidDay =
        new("payment_instruction.invalid_day", "Ödeme günü 1 ile 31 arasında olmalıdır.", ErrorType.Validation);

    public static readonly Error SameAccount =
        new("payment_instruction.same_account", "Gönderen ve alıcı hesap aynı olamaz.", ErrorType.Validation);

    /// <summary>Gönderen hesap yoksa veya müşteriye ait değilse; başkasının hesabının varlığı ele verilmez.</summary>
    public static readonly Error SourceAccountNotFound =
        new("payment_instruction.source_not_found", "Gönderen hesap bulunamadı.", ErrorType.NotFound);

    public static readonly Error TargetAccountNotFound =
        new("payment_instruction.target_not_found", "Alıcı hesap bulunamadı.", ErrorType.NotFound);

    public static readonly Error CurrencyMismatch =
        new("payment_instruction.currency_mismatch", "Hesapların para birimleri farklı.", ErrorType.BusinessRule);
}
