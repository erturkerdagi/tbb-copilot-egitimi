using Bank.Api.Domain;
using Bank.Api.Services;

namespace Bank.Api.Tests.Fakes;

/// <summary>
/// Testler için bellek içi sahte depolar. Gerçek veritabanı gerektirmez.
/// </summary>
public class InMemoryAccountRepository : IAccountRepository
{
    private readonly Dictionary<int, Account> _accounts = new();

    public InMemoryAccountRepository(params Account[] accounts)
    {
        foreach (var account in accounts)
        {
            _accounts[account.Id] = account;
        }
    }

    public Task<Account?> GetByIdAsync(int id, CancellationToken ct) =>
        Task.FromResult(_accounts.GetValueOrDefault(id));
}

public class InMemoryTransferRepository : ITransferRepository
{
    private int _nextId = 1;

    public List<Transfer> Items { get; } = [];

    public Task AddAsync(Transfer transfer, CancellationToken ct)
    {
        transfer.Id = _nextId++;
        Items.Add(transfer);
        return Task.CompletedTask;
    }

    public Task<decimal> GetTotalSentSinceAsync(int accountId, DateTime sinceUtc, CancellationToken ct) =>
        Task.FromResult(Items
            .Where(t => t.FromAccountId == accountId && t.Status == TransferStatus.Completed && t.CreatedAtUtc >= sinceUtc)
            .Sum(t => t.Amount));

    public Task<IReadOnlyList<Transfer>> GetByAccountAsync(int accountId, CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<Transfer>>(Items
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.CreatedAtUtc)
            .ToList());
}

public class FakeUnitOfWork : IUnitOfWork
{
    public int SaveCount { get; private set; }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        SaveCount++;
        return Task.CompletedTask;
    }
}

/// <summary>Testlerde "şimdi"yi sabitlemek için.</summary>
public class FixedTimeProvider(DateTimeOffset now) : TimeProvider
{
    public DateTimeOffset Now { get; set; } = now;

    public override DateTimeOffset GetUtcNow() => Now;
}
