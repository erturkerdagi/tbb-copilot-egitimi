using Bank.Api.Contracts;
using Bank.Api.Domain;
using Bank.Api.Services;
using Bank.Api.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace Bank.Api.Tests;

/// <summary>
/// Modül 4 (edge case keşfi): Günlük limit Türkiye saatine göre gece yarısında sıfırlanmalı.
/// Türkiye saati = UTC + 3.
/// </summary>
public class TransferServiceGunSiniriTests
{
    private readonly Account _from = new(1, "TR000000000000000000000001", 1, "TRY", 100_000m, 10_000m);
    private readonly Account _to = new(2, "TR000000000000000000000002", 2, "TRY", 0m, 10_000m);
    private readonly FixedTimeProvider _time = new(DateTimeOffset.MinValue);
    private readonly TransferService _service;

    public TransferServiceGunSiniriTests()
    {
        _service = new TransferService(
            new InMemoryAccountRepository(_from, _to),
            new InMemoryTransferRepository(),
            new FakeUnitOfWork(),
            new TransferLimitChecker(),
            _time,
            NullLogger<TransferService>.Instance);
    }

    private Task<Bank.Api.Common.Result<TransferResponse>> Gonder(decimal amount, string utcZaman)
    {
        _time.Now = DateTimeOffset.Parse(utcZaman);
        return _service.TransferAsync(new TransferRequest(1, 2, amount, null), CancellationToken.None);
    }

    [Fact]
    public async Task TransferAsync_LimiteTamEsitTutar_Basarili()
    {
        var result = await Gonder(10_000m, "2026-11-03T10:00:00Z");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task TransferAsync_OncekiGununTransferleri_LimiteSayilmaz()
    {
        await Gonder(9_000m, "2026-11-03T10:00:00Z");

        var result = await Gonder(9_000m, "2026-11-04T10:00:00Z");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task TransferAsync_TurkiyeGeceYarisiGectiyse_YeniGunBaslar()
    {
        // 3 Kasım 23:30 TR
        await Gonder(9_000m, "2026-11-03T20:30:00Z");

        // 4 Kasım 00:30 TR (UTC'de hâlâ 3 Kasım)
        var result = await Gonder(9_000m, "2026-11-03T21:30:00Z");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task TransferAsync_UtcGunuDegisseBileTurkiyeGunuAyniysa_LimitDevamEder()
    {
        // 4 Kasım 01:00 TR (UTC'de 3 Kasım)
        await Gonder(9_000m, "2026-11-03T22:00:00Z");

        // 4 Kasım 08:00 TR
        var result = await Gonder(2_000m, "2026-11-04T05:00:00Z");

        Assert.Equal(TransferErrors.DailyLimitExceeded, result.Error);
    }
}
