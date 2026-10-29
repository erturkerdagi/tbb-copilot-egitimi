namespace Bank.Api.Contracts;

public sealed record AccountResponse(
    int Id,
    string Iban,
    int CustomerId,
    string Currency,
    decimal Balance,
    decimal DailyTransferLimit);

public sealed record TransferRequest(
    int FromAccountId,
    int ToAccountId,
    decimal Amount,
    string? Description);

public sealed record TransferResponse(
    int Id,
    int FromAccountId,
    int ToAccountId,
    decimal Amount,
    string Currency,
    string? Description,
    string Status,
    DateTime CreatedAtUtc);
