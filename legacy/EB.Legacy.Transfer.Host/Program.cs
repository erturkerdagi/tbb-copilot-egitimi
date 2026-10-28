using System;
using System.Collections.Generic;
using System.IO;
using EB.Legacy.Transfer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EB.Legacy.Transfer.Host
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            Console.WriteLine("Erdagi Bank - EFT Talimat Isleme v3.0 (.NET 10)");
            Console.WriteLine("===============================================");

            // App.config yerine appsettings.json + ortam degiskenleri.
            // Veritabani baglantisi koda/dosyaya yazilmaz: EftIslem__EftDbBaglantisi ortam degiskeniyle verilir.
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            EftUcretAyarlari ucretAyarlari = config.GetSection(EftUcretAyarlari.Bolum).Get<EftUcretAyarlari>() ?? EftUcretAyarlari.Varsayilan();
            EftIslemAyarlari islemAyarlari = config.GetSection(EftIslemAyarlari.Bolum).Get<EftIslemAyarlari>() ?? new EftIslemAyarlari();

            string dosya = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "ornek-talimatlar.json");
            if (!File.Exists(dosya))
            {
                Console.WriteLine("Talimat dosyasi bulunamadi: " + dosya);
                return 1;
            }

            List<EftTalimati> talimatlar = JsonYardimci.TalimatlariOku(dosya);
            TalimatIsleyici isleyici = new TalimatIsleyici(
                new EftUcretHesaplayici(ucretAyarlari),
                Options.Create(islemAyarlari),
                TimeProvider.System);
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
