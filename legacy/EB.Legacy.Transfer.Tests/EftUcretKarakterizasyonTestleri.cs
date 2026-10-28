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

        [Fact]
        public void KucukTutar_Internet_SabitUcretAlinir()
        {
            Assert.Equal(5.50m, _hesaplayici.Hesapla(750m, "INTERNET"));
        }

        // Eğitimde Copilot ile bu sınıfa yeni karakterizasyon testleri eklenecek.
    }
}
