namespace Bank.Api.Domain;

/// <summary>
/// Banka müşterisi. Ad ve TCKN kişisel veridir; loglara ve hata mesajlarına yazılmaz.
/// </summary>
public class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    /// <summary>T.C. Kimlik Numarası (demo verisi, gerçek değildir).</summary>
    public string NationalId { get; set; } = string.Empty;

    public List<Account> Accounts { get; set; } = [];
}
