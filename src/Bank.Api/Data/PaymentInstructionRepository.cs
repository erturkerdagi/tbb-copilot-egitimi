using Bank.Api.Domain;
using Bank.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Data;

public class EfPaymentInstructionRepository(BankDbContext db) : IPaymentInstructionRepository
{
    public async Task AddAsync(PaymentInstruction instruction, CancellationToken ct) =>
        await db.PaymentInstructions.AddAsync(instruction, ct);

    public async Task<IReadOnlyList<PaymentInstruction>> GetByCustomerAsync(int customerId, CancellationToken ct) =>
        await db.PaymentInstructions
            .Where(i => i.CustomerId == customerId)
            .OrderBy(i => i.Id)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<PaymentInstruction>> GetActiveAsync(CancellationToken ct) =>
        await db.PaymentInstructions
            .Where(i => i.IsActive)
            .OrderBy(i => i.Id)
            .ToListAsync(ct);

    public Task<bool> HasRunAsync(int instructionId, int periodYear, int periodMonth, CancellationToken ct) =>
        db.PaymentInstructionRuns.AnyAsync(
            r => r.InstructionId == instructionId && r.PeriodYear == periodYear && r.PeriodMonth == periodMonth, ct);

    public async Task AddRunAsync(PaymentInstructionRun run, CancellationToken ct) =>
        await db.PaymentInstructionRuns.AddAsync(run, ct);
}
