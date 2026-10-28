using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// Gun ici EFT talimatlarini isler: ucreti hesaplar, mesai kontrolu yapar.
    /// (Modernizasyon: DateTime.Now yerine TimeProvider, ConfigurationManager yerine IOptions.)
    /// </summary>
    public class TalimatIsleyici
    {
        private static readonly TimeSpan TurkiyeSaatFarki = TimeSpan.FromHours(3);

        private readonly EftUcretHesaplayici _hesaplayici;
        private readonly EftIslemAyarlari _ayar;
        private readonly TimeProvider _zaman;

        public TalimatIsleyici(EftUcretHesaplayici hesaplayici, IOptions<EftIslemAyarlari> ayar, TimeProvider zaman)
        {
            _hesaplayici = hesaplayici;
            _ayar = ayar.Value;
            _zaman = zaman;
        }

        public List<EftSonuc> Isle(List<EftTalimati> talimatlar)
        {
            List<EftSonuc> sonuclar = new List<EftSonuc>();

            // Eski kod sunucunun yerel saatini (Turkiye) kullaniyordu; ayni davranis acikca Turkiye saatiyle korunuyor.
            DateTime simdi = _zaman.GetUtcNow().UtcDateTime + TurkiyeSaatFarki;

            foreach (EftTalimati t in talimatlar)
            {
                EftSonuc s = new EftSonuc();
                s.TalimatNo = t.TalimatNo;
                s.Tutar = t.Tutar;
                s.IslemZamani = simdi;

                try
                {
                    if (t.GonderenIban == null || t.GonderenIban.Length != 26 || !t.GonderenIban.StartsWith("TR"))
                    {
                        s.Durum = "HATA";
                        s.Mesaj = "Gecersiz gonderen IBAN"; // Modernizasyon: mesajdan IBAN cikarildi (kisisel veri)
                        sonuclar.Add(s);
                        continue;
                    }

                    s.Ucret = _hesaplayici.Hesapla(t.Tutar, t.Kanal);
                    s.ToplamTutar = t.Tutar + s.Ucret;

                    // 17:00'dan sonra veya hafta sonu verilen talimatlar ertesi is gunu islenir
                    if (simdi.Hour >= _ayar.MesaiBitisSaati
                        || simdi.DayOfWeek == DayOfWeek.Saturday
                        || simdi.DayOfWeek == DayOfWeek.Sunday)
                    {
                        s.Durum = "ERTESI_GUN";
                        s.Mesaj = "Mesai disi talimat, ertesi is gunu islenecek";
                    }
                    else
                    {
                        s.Durum = "ISLENDI";
                        s.Mesaj = "OK";
                    }

                    // Kisisel veri (ad, IBAN) artik konsola yazilmiyor
                    Console.WriteLine("[EFT] " + t.TalimatNo + " tutar=" + t.Tutar + " ucret=" + s.Ucret);
                }
                catch (Exception ex)
                {
                    s.Durum = "HATA";
                    s.Mesaj = ex.Message;
                }

                sonuclar.Add(s);
            }

            return sonuclar;
        }
    }
}
