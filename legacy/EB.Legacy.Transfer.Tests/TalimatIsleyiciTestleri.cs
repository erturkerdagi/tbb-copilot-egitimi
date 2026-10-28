using System;
using System.Collections.Generic;
using EB.Legacy.Transfer;
using Microsoft.Extensions.Options;
using Xunit;

namespace EB.Legacy.Transfer.Tests
{
    /// <summary>
    /// Modernizasyon sonrası mümkün olan testler: TimeProvider sayesinde mesai kontrolü artık test edilebilir.
    /// </summary>
    public class TalimatIsleyiciTestleri
    {
        private sealed class SabitZaman : TimeProvider
        {
            private readonly DateTimeOffset _utc;
            public SabitZaman(DateTimeOffset utc) { _utc = utc; }
            public override DateTimeOffset GetUtcNow() => _utc;
        }

        private static TalimatIsleyici Isleyici(string utcZaman) =>
            new TalimatIsleyici(
                new EftUcretHesaplayici(EftUcretAyarlari.Varsayilan()),
                Options.Create(new EftIslemAyarlari { MesaiBitisSaati = 17 }),
                new SabitZaman(DateTimeOffset.Parse(utcZaman)));

        private static List<EftTalimati> TekTalimat(string iban = "TR330006100519786457841326") =>
            new List<EftTalimati>
            {
                new EftTalimati { TalimatNo = "T1", GonderenIban = iban, Tutar = 750m, Kanal = "INTERNET" }
            };

        [Fact]
        public void Isle_MesaiIcinde_IslendiDurumuVerir()
        {
            // Salı 10:00 TR = 07:00 UTC
            var sonuc = Isleyici("2026-11-03T07:00:00Z").Isle(TekTalimat());

            Assert.Equal("ISLENDI", sonuc[0].Durum);
            Assert.Equal(755.50m, sonuc[0].ToplamTutar);
        }

        [Fact]
        public void Isle_TamSaat17_ErtesiGunDurumuVerir()
        {
            // Salı 17:00 TR = 14:00 UTC
            var sonuc = Isleyici("2026-11-03T14:00:00Z").Isle(TekTalimat());

            Assert.Equal("ERTESI_GUN", sonuc[0].Durum);
        }

        [Fact]
        public void Isle_Cumartesi_ErtesiGunDurumuVerir()
        {
            // Cumartesi 10:00 TR
            var sonuc = Isleyici("2026-11-07T07:00:00Z").Isle(TekTalimat());

            Assert.Equal("ERTESI_GUN", sonuc[0].Durum);
        }

        [Fact]
        public void Isle_GecersizIban_HataDurumuVerir_MesajdaIbanYerAlmaz()
        {
            var sonuc = Isleyici("2026-11-03T07:00:00Z").Isle(TekTalimat(iban: "TR1234"));

            Assert.Equal("HATA", sonuc[0].Durum);
            Assert.DoesNotContain("TR1234", sonuc[0].Mesaj);
        }
    }
}
