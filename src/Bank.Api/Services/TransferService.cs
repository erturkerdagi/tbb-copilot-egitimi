using Bank.Api.Common;
using Bank.Api.Contracts;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface ITransferService
{
    Task<Result<TransferResponse>> TransferAsync(TransferRequest request, CancellationToken ct);

    Task<Result<IReadOnlyList<TransferResponse>>> GetHistoryAsync(int accountId, CancellationToken ct);
}

public class TransferService(
    IAccountRepository accounts,
    ITransferRepository transfers,
    IUnitOfWork unitOfWork,
    TransferLimitChecker limitChecker,
    TimeProvider timeProvider,
    ILogger<TransferService> logger) : ITransferService
{
    public async Task<Result<TransferResponse>> TransferAsync(TransferRequest request, CancellationToken ct)
    {
        if (request.Amount <= 0)
        {
            return Result<TransferResponse>.Failure(TransferErrors.InvalidAmount);
        }

        if (request.FromAccountId == request.ToAccountId)
        {
            return Result<TransferResponse>.Failure(TransferErrors.SameAccount);
        }

        var from = await accounts.GetByIdAsync(request.FromAccountId, ct);
        if (from is null)
        {
            return Result<TransferResponse>.Failure(TransferErrors.SourceAccountNotFound);
        }

        var to = await accounts.GetByIdAsync(request.ToAccountId, ct);
        if (to is null)
        {
            return Result<TransferResponse>.Failure(TransferErrors.TargetAccountNotFound);
        }

        if (from.Currency != to.Currency)
        {
            return Result<TransferResponse>.Failure(TransferErrors.CurrencyMismatch);
        }

        if (from.Balance < request.Amount)
        {
            return Result<TransferResponse>.Failure(TransferErrors.InsufficientBalance);
        }

        // Basitlik için gün başlangıcı UTC olarak alınıyor.
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var startOfDayUtc = now.Date;
        var sentToday = await transfers.GetTotalSentSinceAsync(from.Id, startOfDayUtc, ct);

        if (!limitChecker.IsWithinDailyLimit(request.Amount, sentToday, from.DailyTransferLimit))
        {
            logger.LogWarning("Günlük limit aşıldı. Hesap: {AccountId}, Tutar: {Amount}", from.Id, request.Amount);
            return Result<TransferResponse>.Failure(TransferErrors.DailyLimitExceeded);
        }

        from.Withdraw(request.Amount);
        to.Deposit(request.Amount);

        var transfer = Transfer.Completed(from.Id, to.Id, request.Amount, from.Currency, request.Description, now);
        await transfers.AddAsync(transfer, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Transfer tamamlandı. Transfer: {TransferId}, Hesap: {AccountId}", transfer.Id, from.Id);

        return Result<TransferResponse>.Success(transfer.ToResponse());
    }

    public async Task<Result<IReadOnlyList<TransferResponse>>> GetHistoryAsync(int accountId, CancellationToken ct)
    {
        var account = await accounts.GetByIdAsync(accountId, ct);
        if (account is null)
        {
            return Result<IReadOnlyList<TransferResponse>>.Failure(AccountErrors.NotFound);
        }

        var items = await transfers.GetByAccountAsync(accountId, ct);
        IReadOnlyList<TransferResponse> response = items.Select(t => t.ToResponse()).ToList();
        return Result<IReadOnlyList<TransferResponse>>.Success(response);
    }
}
