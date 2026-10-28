using Bank.Api.Common;
using Bank.Api.Contracts;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IPaymentInstructionService
{
    Task<Result<PaymentInstructionResponse>> CreateAsync(int customerId, CreatePaymentInstructionRequest request, CancellationToken ct);

    Task<IReadOnlyList<PaymentInstructionResponse>> ListAsync(int customerId, CancellationToken ct);
}

public class PaymentInstructionService(
    IAccountRepository accounts,
    IPaymentInstructionRepository instructions,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider,
    ILogger<PaymentInstructionService> logger) : IPaymentInstructionService
{
    public async Task<Result<PaymentInstructionResponse>> CreateAsync(int customerId, CreatePaymentInstructionRequest request, CancellationToken ct)
    {
        if (request.Amount <= 0)
        {
            return Result<PaymentInstructionResponse>.Failure(PaymentInstructionErrors.InvalidAmount);
        }

        if (request.DayOfMonth is < 1 or > 31)
        {
            return Result<PaymentInstructionResponse>.Failure(PaymentInstructionErrors.InvalidDay);
        }

        if (request.FromAccountId == request.ToAccountId)
        {
            return Result<PaymentInstructionResponse>.Failure(PaymentInstructionErrors.SameAccount);
        }

        var from = await accounts.GetByIdAsync(request.FromAccountId, ct);
        if (from is null || from.CustomerId != customerId)
        {
            return Result<PaymentInstructionResponse>.Failure(PaymentInstructionErrors.SourceAccountNotFound);
        }

        var to = await accounts.GetByIdAsync(request.ToAccountId, ct);
        if (to is null)
        {
            return Result<PaymentInstructionResponse>.Failure(PaymentInstructionErrors.TargetAccountNotFound);
        }

        if (from.Currency != to.Currency)
        {
            return Result<PaymentInstructionResponse>.Failure(PaymentInstructionErrors.CurrencyMismatch);
        }

        var instruction = new PaymentInstruction
        {
            CustomerId = customerId,
            FromAccountId = from.Id,
            ToAccountId = to.Id,
            Amount = request.Amount,
            DayOfMonth = request.DayOfMonth,
            Description = request.Description,
            IsActive = true,
            CreatedAtUtc = timeProvider.GetUtcNow().UtcDateTime
        };

        await instructions.AddAsync(instruction, ct);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation("Düzenli ödeme talimatı oluşturuldu. Talimat: {InstructionId}, Hesap: {AccountId}", instruction.Id, from.Id);

        return Result<PaymentInstructionResponse>.Success(instruction.ToResponse());
    }

    public async Task<IReadOnlyList<PaymentInstructionResponse>> ListAsync(int customerId, CancellationToken ct)
    {
        var items = await instructions.GetByCustomerAsync(customerId, ct);
        return items.Select(i => i.ToResponse()).ToList();
    }
}

public static class PaymentInstructionMapping
{
    public static PaymentInstructionResponse ToResponse(this PaymentInstruction i) =>
        new(i.Id, i.CustomerId, i.FromAccountId, i.ToAccountId, i.Amount, i.DayOfMonth, i.Description, i.IsActive);

    public static PaymentRunResponse ToResponse(this PaymentInstructionRun r) =>
        new(r.InstructionId, r.PeriodYear, r.PeriodMonth, r.ExecutionDate, r.Status.ToString(), r.ErrorCode, r.TransferId);
}
