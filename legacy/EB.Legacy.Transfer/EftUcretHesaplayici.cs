using System;
using Microsoft.Extensions.Options;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// EFT islem ucretini hesaplar.
    /// Yazan: M.K. (2014) - Son degisiklik: tarife 2019 - .NET 10 tasimasi: 2026
    ///
    /// BILINEN DAVRANISLAR (karakterizasyon testleriyle kilitlendi, is birimi karari bekleniyor):
    ///  - Hesap double ile yapiliyor ve Math.Round varsayilan olarak "banker's rounding" kullaniyor.
    ///    Ornek: 8.230 TL -> matematiksel 12,345 TL, sonuc 12,34 TL.
    ///  - Kanal karsilastirmasi buyuk/kucuk harfe duyarli: "sube" yazilirsa sube ek ucreti eklenmiyor.
    /// Tasima sirasinda davranis BILEREK degistirilmedi.
    /// </summary>
    public class EftUcretHesaplayici
    {
        private readonly EftUcretAyarlari _ayar;

        public EftUcretHesaplayici(IOptions<EftUcretAyarlari> ayar)
            : this(ayar.Value)
        {
        }

        public EftUcretHesaplayici(EftUcretAyarlari ayar)
        {
            _ayar = ayar ?? throw new ArgumentNullException(nameof(ayar));
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

            return Convert.ToDecimal(Math.Round(ucret, 2));
        }
    }
}
