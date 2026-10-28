using Bank.Api.Domain;
using Bank.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Data;

public class BankDbContext(DbContextOptions<BankDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Transfer> Transfers => Set<Transfer>();

    Task IUnitOfWork.SaveChangesAsync(CancellationToken ct) => SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(e =>
        {
            e.Property(c => c.FullName).HasMaxLength(200).IsRequired();
            e.Property(c => c.NationalId).HasMaxLength(11).IsRequired();
            e.HasMany(c => c.Accounts).WithOne(a => a.Customer).HasForeignKey(a => a.CustomerId);
        });

        modelBuilder.Entity<Account>(e =>
        {
            e.Property(a => a.Iban).HasMaxLength(34).IsRequired();
            e.HasIndex(a => a.Iban).IsUnique();
            e.Property(a => a.Currency).HasMaxLength(3).IsRequired();
            e.Property(a => a.Balance);
            e.Property(a => a.DailyTransferLimit);
        });

        modelBuilder.Entity<Transfer>(e =>
        {
            e.Property(t => t.Currency).HasMaxLength(3).IsRequired();
            e.Property(t => t.Description).HasMaxLength(140);
            e.HasIndex(t => new { t.FromAccountId, t.CreatedAtUtc });
        });
    }
}
