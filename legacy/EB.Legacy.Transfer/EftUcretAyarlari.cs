namespace EB.Legacy.Transfer
{
    /// <summary>
    /// EFT ucret tarifesi parametreleri. appsettings.json icindeki "EftUcret" bolumunden baglanir.
    /// (Modernizasyon: alanlar, yapilandirma baglamasi icin ozellige cevrildi; degerler ve tipler degismedi.)
    /// </summary>
    public class EftUcretAyarlari
    {
        public const string Bolum = "EftUcret";

        public decimal KucukTutarSiniri { get; set; }
        public double SabitUcret { get; set; }
        public decimal BuyukTutarSiniri { get; set; }
        public double OrtaOran { get; set; }
        public double MinUcret { get; set; }
        public double BuyukOran { get; set; }
        public double MaxUcret { get; set; }
        public double SubeEkUcret { get; set; }
        public double MobilIndirimOrani { get; set; }

        /// <summary>
        /// 2019 tarifesi.
        /// </summary>
        public static EftUcretAyarlari Varsayilan()
        {
            return new EftUcretAyarlari
            {
                KucukTutarSiniri = 1000m,
                SabitUcret = 5.50,
                BuyukTutarSiniri = 50000m,
                OrtaOran = 0.0015,
                MinUcret = 8,
                BuyukOran = 0.001,
                MaxUcret = 150,
                SubeEkUcret = 2.00,
                MobilIndirimOrani = 0.25
            };
        }
    }

    /// <summary>
    /// Talimat isleme ayarlari. appsettings.json icindeki "EftIslem" bolumunden baglanir.
    /// </summary>
    public class EftIslemAyarlari
    {
        public const string Bolum = "EftIslem";

        public int MesaiBitisSaati { get; set; } = 17;

        public string EftDbBaglantisi { get; set; }
    }
}
