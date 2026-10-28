namespace Bank.Api.Domain;

public enum TransferStatus
{
    Completed = 1,
    Rejected = 2
}

public enum TransferKind
{
    /// <summary>Başka bir müşteriye ya da başka bir müşterinin hesabına transfer. Günlük limite tabidir.</summary>
    External = 1,

    /// <summary>Müşterinin kendi hesapları arasında virman. Günlük limite tabi değildir.</summary>
    Internal = 2
}

/// <summary>
/// Bir hesaptan diğerine yapılan para transferi kaydı.
/// </summary>
public class Transfer
{
    public int Id { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "TRY";

    public string? Description { get; set; }

    public TransferStatus Status { get; set; }

    public TransferKind Kind { get; set; } = TransferKind.External;

    /// <summary>İşlem zamanı (UTC).</summary>
    public DateTime CreatedAtUtc { get; set; }

    public static Transfer Completed(int fromAccountId, int toAccountId, decimal amount, string currency, string? description, DateTime createdAtUtc) =>
        new()
        {
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            Currency = currency,
            Description = description,
            Status = TransferStatus.Completed,
            Kind = TransferKind.External,
            CreatedAtUtc = createdAtUtc
        };

    public static Transfer CompletedInternal(int fromAccountId, int toAccountId, decimal amount, string currency, string? description, DateTime createdAtUtc) =>
        new()
        {
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            Currency = currency,
            Description = description,
            Status = TransferStatus.Completed,
            Kind = TransferKind.Internal,
            CreatedAtUtc = createdAtUtc
        };
}
