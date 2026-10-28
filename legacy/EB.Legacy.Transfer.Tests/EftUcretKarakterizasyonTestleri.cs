using System;
using EB.Legacy.Transfer;
using Xunit;

namespace EB.Legacy.Transfer.Tests
{
    /// <summary>
    /// Karakterizasyon testleri: Kodun DOĞRU davranışını değil, BUGÜNKÜ davranışını kayıt altına alır.
    /// .NET Framework 4.8 sürümünde yazıldı, .NET 10'a taşındıktan sonra da aynen geçmektedir.
    /// "BİLİNEN DAVRANIŞ" işaretli testler iş biriminin kararını bekleyen tuhaflıklardır.
    /// </summary>
    public class EftUcretKarakterizasyonTestleri
    {
        private readonly EftUcretHesaplayici _hesaplayici = new EftUcretHesaplayici(EftUcretAyarlari.Varsayilan());

        [Fact]
        public void KucukTutar_Internet_SabitUcretAlinir()
        {
            Assert.Equal(5.50m, _hesaplayici.Hesapla(750m, "INTERNET"));
        }

        [Fact]
        public void KucukTutarSiniriAlti_Internet_SabitUcretAlinir()
        {
            Assert.Equal(5.50m, _hesaplayici.Hesapla(999.99m, "INTERNET"));
        }

        [Fact]
        public void KucukTutarSiniriTamBin_Internet_MinimumUcretAlinir()
        {
            // 1.000 TL artık "küçük tutar" değil: %0,15 = 1,50 TL, minimum 8 TL uygulanır.
            Assert.Equal(8.00m, _hesaplayici.Hesapla(1000m, "INTERNET"));
        }

        [Fact]
        public void OrtaTutar_Internet_YuvarlamaAsagiYapilir_BilinenDavranis()
        {
            // BİLİNEN DAVRANIŞ: 8.230 × 0,0015 = 12,345 → double + banker's rounding ile 12,34 (12,35 değil).
            Assert.Equal(12.34m, _hesaplayici.Hesapla(8230m, "INTERNET"));
        }

        [Fact]
        public void OrtaTutar_Mobil_YuzdeYirmiBesIndirimUygulanir()
        {
            Assert.Equal(9.26m, _hesaplayici.Hesapla(8230m, "MOBIL"));
        }

        [Fact]
        public void OrtaTutar_Sube_MinimumUcreteSubeEkUcretiEklenir()
        {
            Assert.Equal(10.00m, _hesaplayici.Hesapla(2500m, "SUBE"));
        }

        [Fact]
        public void KucukHarfSubeKanali_SubeEkUcretiEklenmez_BilinenDavranis()
        {
            // BİLİNEN DAVRANIŞ: Kanal karşılaştırması büyük/küçük harfe duyarlı.
            Assert.Equal(8.00m, _hesaplayici.Hesapla(2500m, "sube"));
        }

        [Fact]
        public void BuyukTutarSiniriTam_OrtaOranUygulanir()
        {
            Assert.Equal(75.00m, _hesaplayici.Hesapla(50000m, "INTERNET"));
        }

        [Fact]
        public void BuyukTutarSiniriniBirKurusAsinca_UcretDuser_BilinenDavranis()
        {
            // BİLİNEN DAVRANIŞ: 50.000,00 TL'nin ücreti 75 TL iken 50.000,01 TL'ninki 50 TL.
            Assert.Equal(50.00m, _hesaplayici.Hesapla(50000.01m, "INTERNET"));
        }

        [Fact]
        public void CokBuyukTutar_MaksimumUcretUygulanir()
        {
            Assert.Equal(150.00m, _hesaplayici.Hesapla(250000m, "INTERNET"));
        }

        [Fact]
        public void CokBuyukTutar_Sube_MaksimumUcreteSubeEkUcretiEklenir()
        {
            Assert.Equal(152.00m, _hesaplayici.Hesapla(250000m, "SUBE"));
        }

        [Fact]
        public void SifirTutar_ArgumentExceptionFirlatir()
        {
            Assert.Throws<ArgumentException>(() => _hesaplayici.Hesapla(0m, "INTERNET"));
        }
    }
}
