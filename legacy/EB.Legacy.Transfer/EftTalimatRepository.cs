using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// EFT talimatlarinin SQL Server'a kaydi.
    /// (Demo ortaminda veritabani yoktur; host uygulamasi bu sinifi kullanmaz.)
    /// </summary>
    public class EftTalimatRepository
    {
        public List<EftTalimati> GonderenIbanaGoreGetir(string iban)
        {
            List<EftTalimati> liste = new List<EftTalimati>();

            using (SqlConnection con = new SqlConnection(Ayarlar.EftDbBaglantisi))
            {
                con.Open();
                string sql = "SELECT TalimatNo, GonderenIban, AliciIban, AliciAdSoyad, Tutar, Kanal, Aciklama, TalimatTarihi " +
                             "FROM EftTalimatlari WHERE GonderenIban = '" + iban + "' ORDER BY TalimatTarihi DESC";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    EftTalimati t = new EftTalimati();
                    t.TalimatNo = dr["TalimatNo"].ToString();
                    t.GonderenIban = dr["GonderenIban"].ToString();
                    t.AliciIban = dr["AliciIban"].ToString();
                    t.AliciAdSoyad = dr["AliciAdSoyad"].ToString();
                    t.Tutar = Convert.ToDecimal(dr["Tutar"]);
                    t.Kanal = dr["Kanal"].ToString();
                    t.Aciklama = dr["Aciklama"].ToString();
                    t.TalimatTarihi = Convert.ToDateTime(dr["TalimatTarihi"]);
                    liste.Add(t);
                }
                dr.Close();
            }

            return liste;
        }

        public void SonucKaydet(EftSonuc s)
        {
            using (SqlConnection con = new SqlConnection(Ayarlar.EftDbBaglantisi))
            {
                con.Open();
                string sql = "INSERT INTO EftSonuclari (TalimatNo, Ucret, ToplamTutar, Durum, Mesaj, IslemZamani) VALUES ('" +
                             s.TalimatNo + "', " + s.Ucret.ToString().Replace(",", ".") + ", " +
                             s.ToplamTutar.ToString().Replace(",", ".") + ", '" + s.Durum + "', '" + s.Mesaj + "', GETDATE())";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
