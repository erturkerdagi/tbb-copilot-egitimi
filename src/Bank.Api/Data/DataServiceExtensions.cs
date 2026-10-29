using Bank.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Bank.Api.Data;

public static class DataServiceExtensions
{
    public static IServiceCollection AddBankData(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("BankDb")
                               ?? throw new InvalidOperationException("ConnectionStrings:BankDb tanımlı değil.");

        services.AddDbContext<BankDbContext>(options => options.UseSqlite(connectionString));
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<BankDbContext>());
        services.AddScoped<IAccountRepository, EfAccountRepository>();
        services.AddScoped<ITransferRepository, EfTransferRepository>();

        return services;
    }

    public static async Task InitializeBankDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BankDbContext>();

        // Development ortamında her açılışta veritabanı sıfırlanır; demolar hep aynı veriyle başlar.
        if (app.Configuration.GetValue<bool>("Database:ResetOnStartup"))
        {
            await db.Database.EnsureDeletedAsync();
        }

        await db.Database.EnsureCreatedAsync();
        SeedData.Populate(db);
    }
}
