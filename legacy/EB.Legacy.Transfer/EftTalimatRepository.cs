using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// EFT talimatlarinin SQL Server'a kaydi.
    /// (Modernizasyon: System.Data.SqlClient yerine Microsoft.Data.SqlClient, parametreli sorgular.)
    /// Demo ortaminda veritabani yoktur; host uygulamasi bu sinifi kullanmaz.
    /// </summary>
    public class EftTalimatRepository
    {
        private readonly string _baglanti;

        public EftTalimatRepository(IOptions<EftIslemAyarlari> ayar)
        {
            _baglanti = ayar.Value.EftDbBaglantisi
                        ?? throw new InvalidOperationException("EftIslem:EftDbBaglantisi tanimli degil.");
        }

        public List<EftTalimati> GonderenIbanaGoreGetir(string iban)
        {
            List<EftTalimati> liste = new List<EftTalimati>();

            using (SqlConnection con = new SqlConnection(_baglanti))
            {
                con.Open();
                const string sql = "SELECT TalimatNo, GonderenIban, AliciIban, AliciAdSoyad, Tutar, Kanal, Aciklama, TalimatTarihi " +
                                   "FROM EftTalimatlari WHERE GonderenIban = @iban ORDER BY TalimatTarihi DESC";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@iban", iban);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            liste.Add(new EftTalimati
                            {
                                TalimatNo = dr["TalimatNo"].ToString(),
                                GonderenIban = dr["GonderenIban"].ToString(),
                                AliciIban = dr["AliciIban"].ToString(),
                                AliciAdSoyad = dr["AliciAdSoyad"].ToString(),
                                Tutar = Convert.ToDecimal(dr["Tutar"]),
                                Kanal = dr["Kanal"].ToString(),
                                Aciklama = dr["Aciklama"].ToString(),
                                TalimatTarihi = Convert.ToDateTime(dr["TalimatTarihi"])
                            });
                        }
                    }
                }
            }

            return liste;
        }

        public void SonucKaydet(EftSonuc s)
        {
            using (SqlConnection con = new SqlConnection(_baglanti))
            {
                con.Open();
                const string sql = "INSERT INTO EftSonuclari (TalimatNo, Ucret, ToplamTutar, Durum, Mesaj, IslemZamani) " +
                                   "VALUES (@no, @ucret, @toplam, @durum, @mesaj, SYSDATETIME())";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@no", s.TalimatNo);
                    cmd.Parameters.AddWithValue("@ucret", s.Ucret);
                    cmd.Parameters.AddWithValue("@toplam", s.ToplamTutar);
                    cmd.Parameters.AddWithValue("@durum", s.Durum);
                    cmd.Parameters.AddWithValue("@mesaj", (object)s.Mesaj ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
