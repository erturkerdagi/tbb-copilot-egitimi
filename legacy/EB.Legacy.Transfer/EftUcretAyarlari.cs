namespace EB.Legacy.Transfer
{
    /// <summary>
    /// EFT ucret tarifesi parametreleri.
    /// Degerler normalde App.config / Web.config appSettings bolumunden okunur (bkz. Ayarlar).
    /// </summary>
    public class EftUcretAyarlari
    {
        public decimal KucukTutarSiniri;   // bu tutarin altinda sabit ucret
        public double SabitUcret;
        public decimal BuyukTutarSiniri;   // bu tutarin ustunde buyuk tutar orani
        public double OrtaOran;
        public double MinUcret;
        public double BuyukOran;
        public double MaxUcret;
        public double SubeEkUcret;
        public double MobilIndirimOrani;

        /// <summary>
        /// 2019 tarifesi. Config okunamazsa bu degerler kullanilir.
        /// </summary>
        public static EftUcretAyarlari Varsayilan()
        {
            EftUcretAyarlari a = new EftUcretAyarlari();
            a.KucukTutarSiniri = 1000m;
            a.SabitUcret = 5.50;
            a.BuyukTutarSiniri = 50000m;
            a.OrtaOran = 0.0015;
            a.MinUcret = 8;
            a.BuyukOran = 0.001;
            a.MaxUcret = 150;
            a.SubeEkUcret = 2.00;
            a.MobilIndirimOrani = 0.25;
            return a;
        }
    }
}
