using Bank.Api.Services;

namespace Bank.Api.Tests;

public class TransferLimitCheckerTests
{
    private readonly TransferLimitChecker _checker = new();

    [Fact]
    public void IsWithinDailyLimit_TutarLimitinAltinda_TrueDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 5_000m, alreadySentToday: 0m, dailyLimit: 10_000m);

        Assert.True(result);
    }

    [Fact]
    public void IsWithinDailyLimit_TutarLimitinUstunde_FalseDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 15_000m, alreadySentToday: 0m, dailyLimit: 10_000m);

        Assert.False(result);
    }

    [Fact]
    public void IsWithinDailyLimit_OncekiTransferlerleBirlikteLimitAsiliyor_FalseDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 3_000m, alreadySentToday: 8_000m, dailyLimit: 10_000m);

        Assert.False(result);
    }

    [Fact]
    public void IsWithinDailyLimit_KucukTutar_TrueDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 100m, alreadySentToday: 0m, dailyLimit: 10_000m);

        Assert.True(result);
    }

    // Modül 4 (TDD): Müşteri şikâyeti — "Limitim 10.000 TL, tam 10.000 TL gönderemiyorum."
    [Fact]
    public void IsWithinDailyLimit_TutarLimiteTamEsit_TrueDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 10_000m, alreadySentToday: 0m, dailyLimit: 10_000m);

        Assert.True(result);
    }

    [Fact]
    public void IsWithinDailyLimit_OncekilerleBirlikteLimiteTamEsit_TrueDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 2_000m, alreadySentToday: 8_000m, dailyLimit: 10_000m);

        Assert.True(result);
    }

    [Fact]
    public void IsWithinDailyLimit_LimitinBirKurusUstu_FalseDoner()
    {
        var result = _checker.IsWithinDailyLimit(amount: 10_000.01m, alreadySentToday: 0m, dailyLimit: 10_000m);

        Assert.False(result);
    }
}
