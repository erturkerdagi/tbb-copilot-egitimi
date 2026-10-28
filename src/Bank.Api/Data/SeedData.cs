using Bank.Api.Domain;

namespace Bank.Api.Data;

/// <summary>
/// Demo verileri. Tüm isimler, TCKN'ler ve IBAN'lar kurgusaldır.
/// </summary>
public static class SeedData
{
    public static void Populate(BankDbContext db)
    {
        if (db.Customers.Any())
        {
            return;
        }

        var ayse = new Customer { Id = 1, FullName = "Ayşe Yılmaz", NationalId = "10000000146" };
        var mehmet = new Customer { Id = 2, FullName = "Mehmet Demir", NationalId = "20000000046" };
        var zeynep = new Customer { Id = 3, FullName = "Zeynep Kaya", NationalId = "30000000082" };

        db.Customers.AddRange(ayse, mehmet, zeynep);

        db.Accounts.AddRange(
            // Ayşe'nin iki TL hesabı: vadesiz ve birikim
            new Account(1, "TR330006100519786457841326", 1, "TRY", 25_000m, 10_000m),
            new Account(2, "TR320010009999901234567890", 1, "TRY", 100_000m, 10_000m),
            // Mehmet'in TL hesabı
            new Account(3, "TR960006400000112345678901", 2, "TRY", 5_000m, 10_000m),
            // Zeynep'in TL ve EUR hesapları
            new Account(4, "TR470001000100000000012345", 3, "TRY", 50_000m, 20_000m),
            new Account(5, "TR180001000100000000067890", 3, "EUR", 3_000m, 5_000m));

        db.SaveChanges();
    }
}
