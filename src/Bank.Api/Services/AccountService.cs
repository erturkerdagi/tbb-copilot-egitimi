using Bank.Api.Common;
using Bank.Api.Contracts;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IAccountService
{
    Task<Result<AccountResponse>> GetAsync(int id, CancellationToken ct);
}

public class AccountService(IAccountRepository accounts) : IAccountService
{
    public async Task<Result<AccountResponse>> GetAsync(int id, CancellationToken ct)
    {
        var account = await accounts.GetByIdAsync(id, ct);
        return account is null
            ? Result<AccountResponse>.Failure(AccountErrors.NotFound)
            : Result<AccountResponse>.Success(account.ToResponse());
    }
}

public static class MappingExtensions
{
    public static AccountResponse ToResponse(this Account a) =>
        new(a.Id, a.Iban, a.CustomerId, a.Currency, a.Balance, a.DailyTransferLimit);

    public static TransferResponse ToResponse(this Transfer t) =>
        new(t.Id, t.FromAccountId, t.ToAccountId, t.Amount, t.Currency, t.Description, t.Status.ToString(), t.CreatedAtUtc);
}
