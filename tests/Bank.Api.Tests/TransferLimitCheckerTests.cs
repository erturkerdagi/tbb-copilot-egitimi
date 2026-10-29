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
}
