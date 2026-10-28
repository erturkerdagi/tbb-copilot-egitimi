using Bank.Api.Common;
using Bank.Api.Contracts;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IInternalTransferService
{
    Task<Result<TransferResponse>> TransferAsync(int customerId, InternalTransferRequest request, CancellationToken ct);
}

/// <summary>
/// Müşterinin kendi hesapları arasında virman (spec: 001-kendi-hesaplar-arasi-virman).
/// Günlük transfer limitine tabi değildir (FR-007).
/// </summary>
public class InternalTransferService(
    IAccountRepository accounts,
    ITransferRepository transfers,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<InternalTransferService> logger) : IInternalTransferService
{
    public async Task<Result<TransferResponse>> TransferAsync(int customerId, InternalTransferRequest request, CancellationToken ct)
    {
        // FR-003
        if (request.Amount <= 0)
        {
            return Result<TransferResponse>.Failure(InternalTransferErrors.InvalidAmount);
        }

        // FR-004
        if (request.FromAccountId == request.ToAccountId)
        {
            return Result<TransferResponse>.Failure(InternalTransferErrors.SameAccount);
        }

        // FR-002: iki hesap da bu müşteriye ait olmalı
        var from = await accounts.GetByIdAsync(request.FromAccountId, ct);
        var to = await accounts.GetByIdAsync(request.ToAccountId, ct);
        if (from is null || to is null || from.CustomerId != customerId || to.CustomerId != customerId)
        {
            return Result<TransferResponse>.Failure(InternalTransferErrors.AccountNotFound);
        }

        // FR-006
        if (from.Currency != to.Currency)
        {
            return Result<TransferResponse>.Failure(InternalTransferErrors.CurrencyMismatch);
        }

        // FR-005
        if (from.Balance < request.Amount)
        {
            return Result<TransferResponse>.Failure(InternalTransferErrors.InsufficientBalance);
        }

        from.Withdraw(request.Amount);
        to.Deposit(request.Amount);

        // FR-008: virman olarak işaretlenir, limit hesabına katılmaz
        var now = timeProvider.GetUtcNow().UtcDateTime;
        var transfer = Transfer.CompletedInternal(from.Id, to.Id, request.Amount, from.Currency, request.Description, now);
        await transfers.AddAsync(transfer, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Virman tamamlandı. Transfer: {TransferId}, Kaynak: {FromAccountId}, Hedef: {ToAccountId}",
            transfer.Id, from.Id, to.Id);

        return Result<TransferResponse>.Success(transfer.ToResponse());
    }
}
