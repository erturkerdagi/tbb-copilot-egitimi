using Bank.Api.Services;

namespace Bank.Api.Tests;

/// <summary>
/// spec 002 — FR-005 (kısa ay) ve FR-006 (hafta sonu kaydırması)
/// </summary>
public class ScheduleCalculatorTests
{
    [Fact]
    public void ExecutionDateFor_HaftaIciGun_AyniGunDoner()
    {
        // 3 Kasım 2026 Salı
        Assert.Equal(new DateOnly(2026, 11, 3), ScheduleCalculator.ExecutionDateFor(3, 2026, 11));
    }

    [Fact]
    public void ExecutionDateFor_Pazar_PazartesiyeKayar()
    {
        // 15 Kasım 2026 Pazar → 16 Kasım Pazartesi
        Assert.Equal(new DateOnly(2026, 11, 16), ScheduleCalculator.ExecutionDateFor(15, 2026, 11));
    }

    [Fact]
    public void ExecutionDateFor_Cumartesi_PazartesiyeKayar()
    {
        // 7 Kasım 2026 Cumartesi → 9 Kasım Pazartesi
        Assert.Equal(new DateOnly(2026, 11, 9), ScheduleCalculator.ExecutionDateFor(7, 2026, 11));
    }

    [Fact]
    public void ExecutionDateFor_Ayin31i_OtuzGunlukAydaSonGun()
    {
        // Kasım 30 gün; 30 Kasım 2026 Pazartesi
        Assert.Equal(new DateOnly(2026, 11, 30), ScheduleCalculator.ExecutionDateFor(31, 2026, 11));
    }

    [Fact]
    public void ExecutionDateFor_AySonuHaftaSonu_SonrakiAyaTasar()
    {
        // 30 Mayıs 2026 Cumartesi → 1 Haziran Pazartesi
        Assert.Equal(new DateOnly(2026, 6, 1), ScheduleCalculator.ExecutionDateFor(30, 2026, 5));
    }

    [Fact]
    public void ExecutionDateFor_SubattaAyin29u_ArtikYilDegilse28ineVeHaftaSonuysaMartaKayar()
    {
        // Şubat 2027: 28 gün, 28 Şubat Pazar → 1 Mart Pazartesi
        Assert.Equal(new DateOnly(2027, 3, 1), ScheduleCalculator.ExecutionDateFor(29, 2027, 2));
    }

    [Fact]
    public void ExecutionDateFor_ArtikYildaSubat29_AyniGun()
    {
        // 29 Şubat 2028 Salı
        Assert.Equal(new DateOnly(2028, 2, 29), ScheduleCalculator.ExecutionDateFor(29, 2028, 2));
    }

    [Fact]
    public void ExecutionDateFor_GecersizGun_HataFirlatir()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ScheduleCalculator.ExecutionDateFor(32, 2026, 11));
    }
}
