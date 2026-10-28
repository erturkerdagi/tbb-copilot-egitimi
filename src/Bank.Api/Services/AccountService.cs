using Bank.Api.Common;
using Bank.Api.Contracts;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IAccountService
{
    Task<Result<AccountResponse>> GetAsync(int id, CancellationToken ct);

    Task<Result<DepositResponse>> DepositAsync(int accountId, DepositRequest request, CancellationToken ct);
}

public class AccountService(
    IAccountRepository accounts,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<AccountService> logger) : IAccountService
{
    public async Task<Result<AccountResponse>> GetAsync(int id, CancellationToken ct)
    {
        var account = await accounts.GetByIdAsync(id, ct);
        return account is null
            ? Result<AccountResponse>.Failure(AccountErrors.NotFound)
            : Result<AccountResponse>.Success(account.ToResponse());
    }

    public async Task<Result<DepositResponse>> DepositAsync(int accountId, DepositRequest request, CancellationToken ct)
    {
        if (request.Amount <= 0)
        {
            return Result<DepositResponse>.Failure(DepositErrors.InvalidAmount);
        }

        if (request.Amount > DepositErrors.MaxSingleDeposit)
        {
            return Result<DepositResponse>.Failure(DepositErrors.AmountTooLarge);
        }

        var account = await accounts.GetByIdAsync(accountId, ct);
        if (account is null)
        {
            return Result<DepositResponse>.Failure(AccountErrors.NotFound);
        }

        account.Deposit(request.Amount);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Para yatırıldı. Hesap: {AccountId}", account.Id);

        var now = timeProvider.GetUtcNow().UtcDateTime;
        return Result<DepositResponse>.Success(new DepositResponse(account.Id, request.Amount, account.Balance, now));
    }
}

public static class MappingExtensions
{
    public static AccountResponse ToResponse(this Account a) =>
        new(a.Id, a.Iban, a.CustomerId, a.Currency, a.Balance, a.DailyTransferLimit);

    public static TransferResponse ToResponse(this Transfer t) =>
        new(t.Id, t.FromAccountId, t.ToAccountId, t.Amount, t.Currency, t.Description, t.Status.ToString(), t.CreatedAtUtc);
}
