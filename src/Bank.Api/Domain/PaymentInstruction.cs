namespace Bank.Api.Domain;

/// <summary>
/// Düzenli ödeme talimatı: her ay belirli bir günde, belirli bir hesaba, belirli tutarda transfer.
/// spec: specs/002-duzenli-odeme-talimati/spec.md
/// </summary>
public class PaymentInstruction
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public decimal Amount { get; set; }

    /// <summary>Ayın kaçıncı günü (1–31). Ay o kadar uzun değilse ayın son günü kullanılır.</summary>
    public int DayOfMonth { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; }
}

public enum PaymentRunStatus
{
    Succeeded = 1,
    Failed = 2
}

/// <summary>
/// Bir talimatın belirli bir dönem (yıl/ay) için çalıştırılma kaydı. Her dönem için en fazla bir kayıt olur.
/// </summary>
public class PaymentInstructionRun
{
    public int Id { get; set; }

    public int InstructionId { get; set; }

    public int PeriodYear { get; set; }

    public int PeriodMonth { get; set; }

    public DateOnly ExecutionDate { get; set; }

    public PaymentRunStatus Status { get; set; }

    public string? ErrorCode { get; set; }

    public int? TransferId { get; set; }
}
