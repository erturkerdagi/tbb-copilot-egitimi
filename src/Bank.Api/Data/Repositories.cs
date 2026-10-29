using Bank.Api.Domain;
using Bank.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Data;

public class EfAccountRepository(BankDbContext db) : IAccountRepository
{
    public Task<Account?> GetByIdAsync(int id, CancellationToken ct) =>
        db.Accounts.FirstOrDefaultAsync(a => a.Id == id, ct);
}

public class EfTransferRepository(BankDbContext db) : ITransferRepository
{
    public async Task AddAsync(Transfer transfer, CancellationToken ct) =>
        await db.Transfers.AddAsync(transfer, ct);

    public async Task<decimal> GetTotalSentSinceAsync(int accountId, DateTime sinceUtc, CancellationToken ct)
    {
        // SQLite, decimal alanlarda SUM'ı veritabanında hesaplayamaz.
        // Bu yüzden tutarlar çekilip toplam bellekte hesaplanıyor.
        var amounts = await db.Transfers
            .Where(t => t.FromAccountId == accountId
                        && t.Status == TransferStatus.Completed
                        && t.CreatedAtUtc >= sinceUtc)
            .Select(t => t.Amount)
            .ToListAsync(ct);

        return amounts.Sum();
    }

    public async Task<IReadOnlyList<Transfer>> GetByAccountAsync(int accountId, CancellationToken ct) =>
        await db.Transfers
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToListAsync(ct);
}
