using System;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// EFT islem ucretini hesaplar.
    /// Yazan: M.K. (2014) - Son degisiklik: tarife 2019
    /// NOT: Ucret hesabi muhasebe ile mutabik, DEGISTIRMEYIN!
    /// </summary>
    public class EftUcretHesaplayici
    {
        private readonly EftUcretAyarlari _ayar;

        public EftUcretHesaplayici()
            : this(Ayarlar.EftUcretAyarlariniYukle())
        {
        }

        public EftUcretHesaplayici(EftUcretAyarlari ayar)
        {
            _ayar = ayar;
        }

        /// <param name="tutar">EFT tutari (TL)</param>
        /// <param name="kanal">SUBE, INTERNET veya MOBIL</param>
        public decimal Hesapla(decimal tutar, string kanal)
        {
            if (tutar <= 0)
                throw new ArgumentException("Tutar sifirdan buyuk olmali");

            double ucret;

            if (tutar < _ayar.KucukTutarSiniri)
            {
                ucret = _ayar.SabitUcret;
            }
            else if (tutar <= _ayar.BuyukTutarSiniri)
            {
                ucret = (double)tutar * _ayar.OrtaOran;
                if (ucret < _ayar.MinUcret)
                    ucret = _ayar.MinUcret;
            }
            else
            {
                ucret = (double)tutar * _ayar.BuyukOran;
                if (ucret > _ayar.MaxUcret)
                    ucret = _ayar.MaxUcret;
            }

            // kanal farki
            if (kanal == "SUBE")
            {
                ucret = ucret + _ayar.SubeEkUcret;
            }
            else if (kanal == "MOBIL")
            {
                ucret = ucret - (ucret * _ayar.MobilIndirimOrani);
            }

            // 2016 internet kampanyasi - iptal edildi
            //if (kanal == "INTERNET")
            //    ucret = ucret * 0.9;

            return Convert.ToDecimal(Math.Round(ucret, 2));
        }
    }
}
