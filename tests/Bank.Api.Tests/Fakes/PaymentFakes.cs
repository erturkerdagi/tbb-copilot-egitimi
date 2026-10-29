using Bank.Api.Domain;
using Bank.Api.Services;

namespace Bank.Api.Tests.Fakes;

public class InMemoryPaymentInstructionRepository : IPaymentInstructionRepository
{
    private int _nextId = 1;

    public List<PaymentInstruction> Items { get; } = [];

    public List<PaymentInstructionRun> Runs { get; } = [];

    public Task AddAsync(PaymentInstruction instruction, CancellationToken ct)
    {
        instruction.Id = _nextId++;
        Items.Add(instruction);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<PaymentInstruction>> GetByCustomerAsync(int customerId, CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<PaymentInstruction>>(Items.Where(i => i.CustomerId == customerId).ToList());

    public Task<IReadOnlyList<PaymentInstruction>> GetActiveAsync(CancellationToken ct) =>
        Task.FromResult<IReadOnlyList<PaymentInstruction>>(Items.Where(i => i.IsActive).ToList());

    public Task<bool> HasRunAsync(int instructionId, int periodYear, int periodMonth, CancellationToken ct) =>
        Task.FromResult(Runs.Any(r => r.InstructionId == instructionId && r.PeriodYear == periodYear && r.PeriodMonth == periodMonth));

    public Task AddRunAsync(PaymentInstructionRun run, CancellationToken ct)
    {
        run.Id = Runs.Count + 1;
        Runs.Add(run);
        return Task.CompletedTask;
    }
}
