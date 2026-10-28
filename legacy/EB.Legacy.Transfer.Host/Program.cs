using System;
using System.Collections.Generic;
using System.IO;
using EB.Legacy.Transfer;

namespace EB.Legacy.Transfer.Host
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Erdagi Bank - EFT Talimat Isleme v2.3");
            Console.WriteLine("=====================================");

            string dosya = args.Length > 0 ? args[0] : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ornek-talimatlar.json");
            if (!File.Exists(dosya))
            {
                Console.WriteLine("Talimat dosyasi bulunamadi: " + dosya);
                return 1;
            }

            List<EftTalimati> talimatlar = JsonYardimci.TalimatlariOku(dosya);
            TalimatIsleyici isleyici = new TalimatIsleyici();
            List<EftSonuc> sonuclar = isleyici.Isle(talimatlar);

            Console.WriteLine();
            Console.WriteLine("TalimatNo   Tutar         Ucret      Toplam        Durum");
            Console.WriteLine("----------  ------------  ---------  ------------  ----------");
            foreach (EftSonuc s in sonuclar)
            {
                Console.WriteLine(string.Format("{0,-10}  {1,12:N2}  {2,9:N2}  {3,12:N2}  {4}",
                    s.TalimatNo, s.Tutar, s.Ucret, s.ToplamTutar, s.Durum));
            }

            Console.WriteLine();
            Console.WriteLine(JsonYardimci.SonuclariYaz(sonuclar));
            return 0;
        }
    }
}
