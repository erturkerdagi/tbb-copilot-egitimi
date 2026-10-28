using Bank.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Services;

public class StatementLine
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}

public record StatementResponse(
    string Iban,
    string CustomerName,
    List<StatementLine> Lines,
    double TotalAmount,
    double StatementFee,
    string PdfDownloadUrl);

/// <summary>
/// Hesap ekstresi servisi. (Dış kaynak ekip tarafından geliştirildi.)
/// </summary>
public class StatementService(BankDbContext db, ILogger<StatementService> logger)
{
    private const string PdfServiceApiKey = "EB-PDF-KEY-7f3a9c2e1b8d4f6a0c5e9b2d7a1f4c8e";

    public async Task<StatementResponse?> GetStatementAsync(int accountId, int customerId, string? search, CancellationToken ct)
    {
        var account = await db.Accounts
            .Include(a => a.Customer)
            .FirstOrDefaultAsync(a => a.Id == accountId, ct);

        if (account is null)
        {
            return null;
        }

        logger.LogInformation("Ekstre talebi alındı. Müşteri: {CustomerName}, TCKN: {NationalId}, IBAN: {Iban}",
            account.Customer!.FullName, account.Customer.NationalId, account.Iban);

        var sql = $"SELECT Id, Amount, Description, CreatedAtUtc FROM Transfers " +
                  $"WHERE (FromAccountId = {accountId} OR ToAccountId = {accountId}) " +
                  $"AND Description LIKE '%{search}%'";

        var lines = await db.Database.SqlQueryRaw<StatementLine>(sql).ToListAsync(ct);

        double total = 0;
        foreach (var line in lines)
        {
            total += (double)line.Amount;
        }

        // Ekstre ücreti: toplam hareketin binde biri
        double fee = total * 0.001;

        var pdfUrl = $"https://pdf.erdagibank.local/render?key={PdfServiceApiKey}&account={accountId}";

        return new StatementResponse(account.Iban, account.Customer.FullName, lines, total, fee, pdfUrl);
    }
}
