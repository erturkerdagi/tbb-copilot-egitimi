using Bank.Api.Contracts;
using Bank.Api.Domain;
using Bank.Api.Services;
using Bank.Api.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bank.Api.Tests;

public class AccountServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 3, 10, 0, 0, TimeSpan.Zero);

    private static (AccountService Service, Account Account, FakeUnitOfWork Uow) CreateService(decimal balance = 1_000m)
    {
        var account = new Account(1, "TR000000000000000000000001", customerId: 1, "TRY", balance, 10_000m);
        var uow = new FakeUnitOfWork();
        var service = new AccountService(
            new InMemoryAccountRepository(account),
            uow,
            new FixedTimeProvider(Now),
            NullLogger<AccountService>.Instance);

        return (service, account, uow);
    }

    [Fact]
    public async Task DepositAsync_GecerliTutar_BakiyeyiArtirir()
    {
        var (service, account, uow) = CreateService(balance: 1_000m);

        var result = await service.DepositAsync(1, new DepositRequest(2_500m, "Maaş"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3_500m, account.Balance);
        Assert.Equal(3_500m, result.Value!.NewBalance);
        Assert.Equal(1, uow.SaveCount);
    }

    [Fact]
    public async Task DepositAsync_SifirTutar_ValidationHatasiDoner()
    {
        var (service, account, uow) = CreateService();

        var result = await service.DepositAsync(1, new DepositRequest(0m, null), CancellationToken.None);

        Assert.Equal(DepositErrors.InvalidAmount, result.Error);
        Assert.Equal(1_000m, account.Balance);
        Assert.Equal(0, uow.SaveCount);
    }

    [Fact]
    public async Task DepositAsync_NegatifTutar_ValidationHatasiDoner()
    {
        var (service, account, _) = CreateService();

        var result = await service.DepositAsync(1, new DepositRequest(-10m, null), CancellationToken.None);

        Assert.Equal(DepositErrors.InvalidAmount, result.Error);
        Assert.Equal(1_000m, account.Balance);
    }

    [Fact]
    public async Task DepositAsync_TekSeferlikLimitinUstunde_HataDoner()
    {
        var (service, _, _) = CreateService();

        var result = await service.DepositAsync(1, new DepositRequest(1_000_000.01m, null), CancellationToken.None);

        Assert.Equal(DepositErrors.AmountTooLarge, result.Error);
    }

    [Fact]
    public async Task DepositAsync_TekSeferlikLimiteTamEsit_KabulEdilir()
    {
        var (service, _, _) = CreateService();

        var result = await service.DepositAsync(1, new DepositRequest(1_000_000m, null), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DepositAsync_HesapYok_NotFoundDoner()
    {
        var (service, _, _) = CreateService();

        var result = await service.DepositAsync(99, new DepositRequest(100m, null), CancellationToken.None);

        Assert.Equal(AccountErrors.NotFound, result.Error);
    }
}
