using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id, CancellationToken ct);
}

public interface ITransferRepository
{
    Task AddAsync(Transfer transfer, CancellationToken ct);

    /// <summary>Hesaptan verilen andan sonra tamamlanmış transferlerin toplam tutarı.</summary>
    Task<decimal> GetTotalSentSinceAsync(int accountId, DateTime sinceUtc, CancellationToken ct);

    Task<IReadOnlyList<Transfer>> GetByAccountAsync(int accountId, CancellationToken ct);
}

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}
