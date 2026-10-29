using Bank.Api.Contracts;
using Bank.Api.Domain;

namespace Bank.Api.Services;

public interface IPaymentInstructionRunner
{
    /// <summary>Verilen bankacılık gününde gerçekleşmesi gereken talimatları çalıştırır.</summary>
    Task<PaymentRunSummaryResponse> RunDueAsync(DateOnly bankDate, CancellationToken ct);
}

/// <summary>
/// Düzenli ödeme talimatlarını çalıştırır (spec 002, FR-007 – FR-010).
/// Gerçek sistemde bu işi zamanlanmış bir iş (job) yapar; demoda bir uç üzerinden tetiklenir.
/// </summary>
public class PaymentInstructionRunner(
    IPaymentInstructionRepository instructions,
    ITransferService transferService,
    IUnitOfWork unitOfWork,
    ILogger<PaymentInstructionRunner> logger) : IPaymentInstructionRunner
{
    public async Task<PaymentRunSummaryResponse> RunDueAsync(DateOnly bankDate, CancellationToken ct)
    {
        var runs = new List<PaymentInstructionRun>();
        var active = await instructions.GetActiveAsync(ct);

        // Hafta sonu kaydırması nedeniyle önceki ayın talimatı bu aya taşmış olabilir.
        var current = new DateOnly(bankDate.Year, bankDate.Month, 1);
        DateOnly[] periods = [current.AddMonths(-1), current];

        foreach (var instruction in active)
        {
            foreach (var period in periods)
            {
                var executionDate = ScheduleCalculator.ExecutionDateFor(instruction.DayOfMonth, period.Year, period.Month);
                if (executionDate != bankDate)
                {
                    continue;
                }

                // FR-009: aynı dönem için en fazla bir kez
                if (await instructions.HasRunAsync(instruction.Id, period.Year, period.Month, ct))
                {
                    continue;
                }

                // FR-007: normal (dış) transfer olarak; bakiye ve günlük limit kontrolleri geçerli
                var result = await transferService.TransferAsync(
                    new TransferRequest(instruction.FromAccountId, instruction.ToAccountId, instruction.Amount,
                        instruction.Description ?? $"Düzenli ödeme #{instruction.Id}"),
                    ct);

                var run = new PaymentInstructionRun
                {
                    InstructionId = instruction.Id,
                    PeriodYear = period.Year,
                    PeriodMonth = period.Month,
                    ExecutionDate = executionDate,
                    Status = result.IsSuccess ? PaymentRunStatus.Succeeded : PaymentRunStatus.Failed,
                    ErrorCode = result.Error?.Code,
                    TransferId = result.Value?.Id
                };

                await instructions.AddRunAsync(run, ct);
                await unitOfWork.SaveChangesAsync(ct);
                runs.Add(run);

                // FR-008: başarısız çalıştırma kaydedilir, aynı dönem içinde tekrar denenmez
                if (!result.IsSuccess)
                {
                    logger.LogWarning("Düzenli ödeme başarısız. Talimat: {InstructionId}, Hata: {ErrorCode}", instruction.Id, run.ErrorCode);
                }
            }
        }

        return new PaymentRunSummaryResponse(
            bankDate,
            runs.Count(r => r.Status == PaymentRunStatus.Succeeded),
            runs.Count(r => r.Status == PaymentRunStatus.Failed),
            runs.Select(r => r.ToResponse()).ToList());
    }
}
