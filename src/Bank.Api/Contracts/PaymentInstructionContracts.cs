namespace Bank.Api.Contracts;

public sealed record CreatePaymentInstructionRequest(
    int FromAccountId,
    int ToAccountId,
    decimal Amount,
    int DayOfMonth,
    string? Description);

public sealed record PaymentInstructionResponse(
    int Id,
    int CustomerId,
    int FromAccountId,
    int ToAccountId,
    decimal Amount,
    int DayOfMonth,
    string? Description,
    bool IsActive);

public sealed record PaymentRunResponse(
    int InstructionId,
    int PeriodYear,
    int PeriodMonth,
    DateOnly ExecutionDate,
    string Status,
    string? ErrorCode,
    int? TransferId);

public sealed record PaymentRunSummaryResponse(
    DateOnly BankDate,
    int Succeeded,
    int Failed,
    IReadOnlyList<PaymentRunResponse> Runs);
