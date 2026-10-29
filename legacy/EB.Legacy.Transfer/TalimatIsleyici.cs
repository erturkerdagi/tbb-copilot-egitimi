using System;
using System.Collections.Generic;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// Gun ici EFT talimatlarini isler: ucreti hesaplar, mesai kontrolu yapar.
    /// </summary>
    public class TalimatIsleyici
    {
        private EftUcretHesaplayici hesaplayici;

        public TalimatIsleyici()
        {
            hesaplayici = new EftUcretHesaplayici();
        }

        public TalimatIsleyici(EftUcretHesaplayici h)
        {
            hesaplayici = h;
        }

        public List<EftSonuc> Isle(List<EftTalimati> talimatlar)
        {
            List<EftSonuc> sonuclar = new List<EftSonuc>();

            foreach (EftTalimati t in talimatlar)
            {
                EftSonuc s = new EftSonuc();
                s.TalimatNo = t.TalimatNo;
                s.Tutar = t.Tutar;
                s.IslemZamani = DateTime.Now;

                try
                {
                    if (t.GonderenIban == null || t.GonderenIban.Length != 26 || !t.GonderenIban.StartsWith("TR"))
                    {
                        s.Durum = "HATA";
                        s.Mesaj = "Gecersiz gonderen IBAN: " + t.GonderenIban;
                        sonuclar.Add(s);
                        continue;
                    }

                    s.Ucret = hesaplayici.Hesapla(t.Tutar, t.Kanal);
                    s.ToplamTutar = t.Tutar + s.Ucret;

                    // 17:00'dan sonra veya hafta sonu verilen talimatlar ertesi is gunu islenir
                    if (DateTime.Now.Hour >= Ayarlar.MesaiBitisSaati
                        || DateTime.Now.DayOfWeek == DayOfWeek.Saturday
                        || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                    {
                        s.Durum = "ERTESI_GUN";
                        s.Mesaj = "Mesai disi talimat, ertesi is gunu islenecek";
                    }
                    else
                    {
                        s.Durum = "ISLENDI";
                        s.Mesaj = "OK";
                    }

                    Console.WriteLine("[EFT] " + t.TalimatNo + " " + t.AliciAdSoyad + " " + t.AliciIban + " tutar=" + t.Tutar + " ucret=" + s.Ucret);
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
