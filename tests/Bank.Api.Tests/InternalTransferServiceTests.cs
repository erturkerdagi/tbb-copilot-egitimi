using Bank.Api.Contracts;
using Bank.Api.Domain;
using Bank.Api.Services;
using Bank.Api.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bank.Api.Tests;

/// <summary>
/// spec: specs/001-kendi-hesaplar-arasi-virman/spec.md
/// </summary>
public class InternalTransferServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 11, 3, 10, 0, 0, TimeSpan.Zero);

    private const int Ayse = 1;
    private const int Mehmet = 2;

    private readonly Account _ayseVadesiz = new(1, "TR000000000000000000000001", Ayse, "TRY", 25_000m, 10_000m);
    private readonly Account _ayseBirikim = new(2, "TR000000000000000000000002", Ayse, "TRY", 100_000m, 10_000m);
    private readonly Account _mehmet = new(3, "TR000000000000000000000003", Mehmet, "TRY", 5_000m, 10_000m);
    private readonly Account _ayseEuro = new(4, "TR000000000000000000000004", Ayse, "EUR", 1_000m, 5_000m);

    private readonly InMemoryTransferRepository _transfers = new();
    private readonly InternalTransferService _service;
    private readonly TransferService _externalService;

    public InternalTransferServiceTests()
    {
        var accounts = new InMemoryAccountRepository(_ayseVadesiz, _ayseBirikim, _mehmet, _ayseEuro);
        var uow = new FakeUnitOfWork();
        var time = new FixedTimeProvider(Now);

        _service = new InternalTransferService(accounts, _transfers, uow, time, NullLogger<InternalTransferService>.Instance);
        _externalService = new TransferService(accounts, _transfers, uow, new TransferLimitChecker(), time, NullLogger<TransferService>.Instance);
    }

    // US1 - Senaryo 1
    [Fact]
    public async Task TransferAsync_KendiHesaplariArasinda_BakiyeleriGunceller()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 2, 5_000m, "Birikim"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(20_000m, _ayseVadesiz.Balance);
        Assert.Equal(105_000m, _ayseBirikim.Balance);
        Assert.Equal("Internal", result.Value!.Kind);
    }

    // US1 - Senaryo 2 (FR-007)
    [Fact]
    public async Task TransferAsync_GunlukLimittenBuyukTutar_KabulEdilir()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 2, 15_000m, null), CancellationToken.None);

        Assert.True(result.IsSuccess);
    }

    // US1 - Senaryo 3 (FR-007)
    [Fact]
    public async Task TransferAsync_VirmanSonrasiDisTransfer_LimittenEtkilenmez()
    {
        await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 2, 15_000m, null), CancellationToken.None);

        var external = await _externalService.TransferAsync(new TransferRequest(1, 3, 5_000m, null), CancellationToken.None);

        Assert.True(external.IsSuccess);
    }

    [Fact]
    public async Task TransferAsync_BakiyeninTamami_Aktarilabilir()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 2, 25_000m, null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, _ayseVadesiz.Balance);
    }

    [Fact]
    public async Task TransferAsync_SifirTutar_HataDoner()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 2, 0m, null), CancellationToken.None);

        Assert.Equal(InternalTransferErrors.InvalidAmount, result.Error);
    }

    [Fact]
    public async Task TransferAsync_AyniHesap_HataDoner()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 1, 100m, null), CancellationToken.None);

        Assert.Equal(InternalTransferErrors.SameAccount, result.Error);
    }

    [Fact]
    public async Task TransferAsync_YetersizBakiye_HataDoner()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 2, 25_000.01m, null), CancellationToken.None);

        Assert.Equal(InternalTransferErrors.InsufficientBalance, result.Error);
        Assert.Equal(25_000m, _ayseVadesiz.Balance);
    }

    [Fact]
    public async Task TransferAsync_FarkliParaBirimi_HataDoner()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 4, 100m, null), CancellationToken.None);

        Assert.Equal(InternalTransferErrors.CurrencyMismatch, result.Error);
    }

    // US2 (FR-002)
    [Fact]
    public async Task TransferAsync_KaynakHesapBaskasinaAit_NotFoundDoner()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(3, 1, 100m, null), CancellationToken.None);

        Assert.Equal(InternalTransferErrors.AccountNotFound, result.Error);
        Assert.Equal(5_000m, _mehmet.Balance);
        Assert.Equal(25_000m, _ayseVadesiz.Balance);
    }

    [Fact]
    public async Task TransferAsync_HedefHesapBaskasinaAit_NotFoundDoner()
    {
        var result = await _service.TransferAsync(Ayse, new InternalTransferRequest(1, 3, 100m, null), CancellationToken.None);

        Assert.Equal(InternalTransferErrors.AccountNotFound, result.Error);
        Assert.Empty(_transfers.Items);
    }
}
