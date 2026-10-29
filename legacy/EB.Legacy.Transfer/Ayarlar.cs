using System;
using System.Configuration;
using System.Globalization;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// Uygulama ayarlarina erisim. (2014 - ilk surum, 2019 - tarife guncellemesi)
    /// </summary>
    public static class Ayarlar
    {
        public static string EftDbBaglantisi
        {
            get
            {
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings["EftDb"];
                if (cs == null)
                    throw new ConfigurationErrorsException("EftDb baglanti cumlesi bulunamadi!");
                return cs.ConnectionString;
            }
        }

        public static int MesaiBitisSaati
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["Eft.MesaiBitisSaati"] ?? "17"); }
        }

        public static EftUcretAyarlari EftUcretAyarlariniYukle()
        {
            EftUcretAyarlari v = EftUcretAyarlari.Varsayilan();
            try
            {
                v.KucukTutarSiniri = Ondalik("Eft.KucukTutarSiniri", v.KucukTutarSiniri);
                v.SabitUcret = Kesirli("Eft.SabitUcret", v.SabitUcret);
                v.BuyukTutarSiniri = Ondalik("Eft.BuyukTutarSiniri", v.BuyukTutarSiniri);
                v.OrtaOran = Kesirli("Eft.OrtaOran", v.OrtaOran);
                v.MinUcret = Kesirli("Eft.MinUcret", v.MinUcret);
                v.BuyukOran = Kesirli("Eft.BuyukOran", v.BuyukOran);
                v.MaxUcret = Kesirli("Eft.MaxUcret", v.MaxUcret);
                v.SubeEkUcret = Kesirli("Eft.SubeEkUcret", v.SubeEkUcret);
                v.MobilIndirimOrani = Kesirli("Eft.MobilIndirimOrani", v.MobilIndirimOrani);
            }
            catch (Exception)
            {
                // config bozuksa varsayilan tarife ile devam et
            }
            return v;
        }

        private static decimal Ondalik(string anahtar, decimal varsayilan)
        {
            string s = ConfigurationManager.AppSettings[anahtar];
            if (string.IsNullOrEmpty(s)) return varsayilan;
            return decimal.Parse(s, CultureInfo.InvariantCulture);
        }

        private static double Kesirli(string anahtar, double varsayilan)
        {
            string s = ConfigurationManager.AppSettings[anahtar];
            if (string.IsNullOrEmpty(s)) return varsayilan;
            return double.Parse(s, CultureInfo.InvariantCulture);
        }
    }
}
