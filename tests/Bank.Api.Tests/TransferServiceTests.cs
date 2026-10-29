using Bank.Api.Contracts;
using Bank.Api.Domain;
using Bank.Api.Services;
using Bank.Api.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bank.Api.Tests;

public class TransferServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 3, 10, 0, 0, TimeSpan.Zero);

    private static (TransferService Service, InMemoryTransferRepository Transfers, FakeUnitOfWork Uow) CreateService(params Account[] accounts)
    {
        var transfers = new InMemoryTransferRepository();
        var uow = new FakeUnitOfWork();
        var service = new TransferService(
            new InMemoryAccountRepository(accounts),
            transfers,
            uow,
            new TransferLimitChecker(),
            new FixedTimeProvider(Now),
            NullLogger<TransferService>.Instance);

        return (service, transfers, uow);
    }

    private static Account TlAccount(int id, decimal balance, decimal limit = 10_000m) =>
        new(id, $"TR00000000000000000000000{id}", customerId: id, "TRY", balance, limit);

    [Fact]
    public async Task TransferAsync_GecerliIstek_BakiyeleriGunceller()
    {
        var from = TlAccount(1, 5_000m);
        var to = TlAccount(2, 1_000m);
        var (service, transfers, uow) = CreateService(from, to);

        var result = await service.TransferAsync(new TransferRequest(1, 2, 1_500m, "Test"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(3_500m, from.Balance);
        Assert.Equal(2_500m, to.Balance);
        Assert.Single(transfers.Items);
        Assert.Equal(1, uow.SaveCount);
    }

    [Fact]
    public async Task TransferAsync_YetersizBakiye_HataDoner()
    {
        var (service, _, _) = CreateService(TlAccount(1, 500m), TlAccount(2, 0m));

        var result = await service.TransferAsync(new TransferRequest(1, 2, 1_000m, null), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(TransferErrors.InsufficientBalance, result.Error);
    }

    [Fact]
    public async Task TransferAsync_AyniHesap_HataDoner()
    {
        var (service, _, _) = CreateService(TlAccount(1, 5_000m));

        var result = await service.TransferAsync(new TransferRequest(1, 1, 100m, null), CancellationToken.None);

        Assert.Equal(TransferErrors.SameAccount, result.Error);
    }

    [Fact]
    public async Task TransferAsync_SifirTutar_HataDoner()
    {
        var (service, _, _) = CreateService(TlAccount(1, 5_000m), TlAccount(2, 0m));

        var result = await service.TransferAsync(new TransferRequest(1, 2, 0m, null), CancellationToken.None);

        Assert.Equal(TransferErrors.InvalidAmount, result.Error);
    }

    [Fact]
    public async Task TransferAsync_HesapYok_HataDoner()
    {
        var (service, _, _) = CreateService(TlAccount(1, 5_000m));

        var result = await service.TransferAsync(new TransferRequest(1, 99, 100m, null), CancellationToken.None);

        Assert.Equal(TransferErrors.TargetAccountNotFound, result.Error);
    }

    [Fact]
    public async Task TransferAsync_GunlukLimitAsiliyor_HataDoner()
    {
        var from = TlAccount(1, 50_000m);
        var (service, _, _) = CreateService(from, TlAccount(2, 0m));

        await service.TransferAsync(new TransferRequest(1, 2, 9_000m, null), CancellationToken.None);
        var result = await service.TransferAsync(new TransferRequest(1, 2, 2_000m, null), CancellationToken.None);

        Assert.Equal(TransferErrors.DailyLimitExceeded, result.Error);
    }
}
