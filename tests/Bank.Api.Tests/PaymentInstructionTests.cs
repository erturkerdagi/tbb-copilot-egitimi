using Bank.Api.Contracts;
using Bank.Api.Domain;
using Bank.Api.Services;
using Bank.Api.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bank.Api.Tests;

/// <summary>
/// spec: specs/002-duzenli-odeme-talimati/spec.md
/// </summary>
public class PaymentInstructionTests
{
    private const int Ayse = 1;

    private readonly Account _ayseVadesiz = new(1, "TR000000000000000000000001", Ayse, "TRY", 25_000m, 10_000m);
    private readonly Account _mehmet = new(3, "TR000000000000000000000003", 2, "TRY", 5_000m, 10_000m);
    private readonly Account _zeynepEuro = new(5, "TR000000000000000000000005", 3, "EUR", 3_000m, 5_000m);

    private readonly InMemoryPaymentInstructionRepository _instructions = new();
    private readonly InMemoryTransferRepository _transfers = new();
    private readonly FixedTimeProvider _time = new(new DateTimeOffset(2026, 11, 5, 6, 0, 0, TimeSpan.Zero));
    private readonly PaymentInstructionService _service;
    private readonly PaymentInstructionRunner _runner;

    public PaymentInstructionTests()
    {
        var accounts = new InMemoryAccountRepository(_ayseVadesiz, _mehmet, _zeynepEuro);
        var uow = new FakeUnitOfWork();
        var transferService = new TransferService(accounts, _transfers, uow, new TransferLimitChecker(), _time, NullLogger<TransferService>.Instance);

        _service = new PaymentInstructionService(accounts, _instructions, uow, _time, NullLogger<PaymentInstructionService>.Instance);
        _runner = new PaymentInstructionRunner(_instructions, transferService, uow, NullLogger<PaymentInstructionRunner>.Instance);
    }

    private Task<Bank.Api.Common.Result<PaymentInstructionResponse>> Olustur(decimal amount = 1_500m, int day = 5, int from = 1, int to = 3) =>
        _service.CreateAsync(Ayse, new CreatePaymentInstructionRequest(from, to, amount, day, "Kira"), CancellationToken.None);

    // --- Talimat oluşturma ---

    [Fact]
    public async Task CreateAsync_GecerliTalimat_KaydedilirVeAktifOlur()
    {
        var result = await Olustur();

        Assert.True(result.IsSuccess);
        Assert.True(result.Value!.IsActive);
        Assert.Single(_instructions.Items);
    }

    [Fact]
    public async Task CreateAsync_SifirTutar_HataDoner()
    {
        var result = await Olustur(amount: 0m);

        Assert.Equal(PaymentInstructionErrors.InvalidAmount, result.Error);
    }

    [Fact]
    public async Task CreateAsync_Gun32_HataDoner()
    {
        var result = await Olustur(day: 32);

        Assert.Equal(PaymentInstructionErrors.InvalidDay, result.Error);
    }

    [Fact]
    public async Task CreateAsync_GonderenHesapBaskasinaAit_NotFoundDoner()
    {
        var result = await Olustur(from: 3, to: 1);

        Assert.Equal(PaymentInstructionErrors.SourceAccountNotFound, result.Error);
        Assert.Empty(_instructions.Items);
    }

    [Fact]
    public async Task CreateAsync_FarkliParaBirimi_HataDoner()
    {
        var result = await Olustur(to: 5);

        Assert.Equal(PaymentInstructionErrors.CurrencyMismatch, result.Error);
    }

    // --- Çalıştırma ---

    [Fact]
    public async Task RunDueAsync_GunuGelenTalimat_TransferYapilir()
    {
        await Olustur(amount: 1_500m, day: 5);

        var summary = await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);

        Assert.Equal(1, summary.Succeeded);
        Assert.Equal(23_500m, _ayseVadesiz.Balance);
        Assert.Equal(6_500m, _mehmet.Balance);
        Assert.Single(_transfers.Items);
    }

    [Fact]
    public async Task RunDueAsync_GunuGelmemisTalimat_Calismaz()
    {
        await Olustur(day: 10);

        var summary = await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);

        Assert.Empty(summary.Runs);
        Assert.Equal(25_000m, _ayseVadesiz.Balance);
    }

    [Fact]
    public async Task RunDueAsync_AyniGunIkinciKez_TekrarTransferYapmaz()
    {
        await Olustur(day: 5);

        await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);
        var second = await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);

        Assert.Empty(second.Runs);
        Assert.Single(_transfers.Items);
    }

    [Fact]
    public async Task RunDueAsync_YetersizBakiye_BasarisizKaydedilirVeTekrarDenenmez()
    {
        await Olustur(amount: 30_000m, day: 5);

        var first = await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);
        var retry = await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);

        var run = Assert.Single(first.Runs);
        Assert.Equal("Failed", run.Status);
        Assert.Equal(TransferErrors.InsufficientBalance.Code, run.ErrorCode);
        Assert.Empty(retry.Runs);
        Assert.Equal(25_000m, _ayseVadesiz.Balance);
    }

    [Fact]
    public async Task RunDueAsync_GunlukLimitiAsanTalimat_BasarisizKaydedilir()
    {
        await Olustur(amount: 10_000.01m, day: 5);

        var summary = await _runner.RunDueAsync(new DateOnly(2026, 11, 5), CancellationToken.None);

        Assert.Equal(TransferErrors.DailyLimitExceeded.Code, Assert.Single(summary.Runs).ErrorCode);
    }

    [Fact]
    public async Task RunDueAsync_OncekiAyinHaftaSonunaDenkGelenTalimati_BuAyCalisir()
    {
        // 31 Ocak 2027 Pazar → Ocak dönemi 1 Şubat 2027 Pazartesi çalışır
        await Olustur(amount: 500m, day: 31);

        var summary = await _runner.RunDueAsync(new DateOnly(2027, 2, 1), CancellationToken.None);

        var run = Assert.Single(summary.Runs);
        Assert.Equal(2027, run.PeriodYear);
        Assert.Equal(1, run.PeriodMonth);
        Assert.Equal("Succeeded", run.Status);
    }
}
