using EB.Legacy.Transfer;
using Xunit;

namespace EB.Legacy.Transfer.Tests
{
    /// <summary>
    /// Karakterizasyon testleri: Kodun DOĞRU davranışını değil, BUGÜNKÜ davranışını kayıt altına alır.
    /// Modernizasyondan sonra bu testler aynen geçmelidir; geçmiyorsa davranış değişmiştir.
    /// </summary>
    public class EftUcretKarakterizasyonTestleri
    {
        private readonly EftUcretHesaplayici _hesaplayici = new EftUcretHesaplayici(EftUcretAyarlari.Varsayilan());

        [Theory]
        [InlineData(999.99, "INTERNET", 5.50)]
        [InlineData(999.99, "MOBIL", 4.12)]
        [InlineData(999.99, "SUBE", 7.50)]
        [InlineData(1000, "INTERNET", 8)]
        [InlineData(1000, "MOBIL", 6)]
        [InlineData(1000, "SUBE", 10)]
        [InlineData(2500, "INTERNET", 8)]
        [InlineData(2500, "MOBIL", 6)]
        [InlineData(2500, "SUBE", 10)]
        [InlineData(8230, "INTERNET", 12.34)]
        [InlineData(8230, "MOBIL", 9.26)]
        [InlineData(8230, "SUBE", 14.34)]
        [InlineData(50000, "INTERNET", 75)]
        [InlineData(50000, "MOBIL", 56.25)]
        [InlineData(50000, "SUBE", 77)]
        [InlineData(250000, "INTERNET", 150)]
        [InlineData(250000, "MOBIL", 112.50)]
        [InlineData(250000, "SUBE", 152)]
        public void Hesapla_BilinenDavranis_StandartKanallardakiUcretiKorur(decimal tutar, string kanal, decimal beklenenUcret)
        {
            Assert.Equal(beklenenUcret, _hesaplayici.Hesapla(tutar, kanal));
        }

        [Theory]
        [InlineData("INTERNET", 50)]
        [InlineData("MOBIL", 37.50)]
        [InlineData("SUBE", 52)]
        public void Hesapla_BilinenDavranis_EsikAsimindaUcretDusuyor(string kanal, decimal beklenenUcret)
        {
            Assert.Equal(beklenenUcret, _hesaplayici.Hesapla(50000.01m, kanal));
        }

        [Theory]
        [InlineData(999.99, 5.50)]
        [InlineData(1000, 8)]
        [InlineData(2500, 8)]
        [InlineData(8230, 12.34)]
        [InlineData(50000, 75)]
        [InlineData(50000.01, 50)]
        [InlineData(250000, 150)]
        public void Hesapla_BilinenDavranis_KucukHarfliSubeEkUcretUygulamaz(decimal tutar, decimal beklenenUcret)
        {
            Assert.Equal(beklenenUcret, _hesaplayici.Hesapla(tutar, "sube"));
        }
    }
}
