using System;

namespace EB.Legacy.Transfer
{
    public class EftSonuc
    {
        public string TalimatNo { get; set; }
        public decimal Tutar { get; set; }
        public decimal Ucret { get; set; }
        public decimal ToplamTutar { get; set; }
        public string Durum { get; set; }   // ISLENDI / ERTESI_GUN / HATA
        public string Mesaj { get; set; }
        public DateTime IslemZamani { get; set; }
    }
}
