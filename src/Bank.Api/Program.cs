using Bank.Api.Data;
using Bank.Api.Endpoints;
using Bank.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// Veri erişimi (EF Core + SQLite) ve iş servisleri
builder.Services.AddBankData(builder.Configuration);
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<TransferLimitChecker>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<IAccountService, AccountService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();

// Demo veritabanını oluştur ve örnek verileri yükle
await app.InitializeBankDatabaseAsync();

app.MapGet("/", () => Results.Ok(new { uygulama = "Erdağı Bank Hesap API", durum = "çalışıyor" }));
app.MapAccountEndpoints();
app.MapTransferEndpoints();

app.Run();

// Entegrasyon testlerinin uygulamaya erişebilmesi için
public partial class Program;
