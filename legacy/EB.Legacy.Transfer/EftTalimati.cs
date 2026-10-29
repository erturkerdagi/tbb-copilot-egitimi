using System;

namespace EB.Legacy.Transfer
{
    public class EftTalimati
    {
        public string TalimatNo { get; set; }
        public string GonderenIban { get; set; }
        public string AliciIban { get; set; }
        public string AliciAdSoyad { get; set; }
        public decimal Tutar { get; set; }
        public string Kanal { get; set; }   // SUBE / INTERNET / MOBIL
        public string Aciklama { get; set; }
        public DateTime TalimatTarihi { get; set; }
    }
}
