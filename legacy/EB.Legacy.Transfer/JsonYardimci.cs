using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace EB.Legacy.Transfer
{
    /// <summary>
    /// (Modernizasyon: JavaScriptSerializer yerine System.Text.Json.
    ///  Not: Tarih alanlari artik "\/Date(...)\/" yerine ISO 8601 bicimde yaziliyor.)
    /// </summary>
    public static class JsonYardimci
    {
        private static readonly JsonSerializerOptions Okuma = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions Yazma = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static List<EftTalimati> TalimatlariOku(string dosyaYolu)
        {
            string json = File.ReadAllText(dosyaYolu, Encoding.UTF8);
            return JsonSerializer.Deserialize<List<EftTalimati>>(json, Okuma) ?? new List<EftTalimati>();
        }

        public static string SonuclariYaz(List<EftSonuc> sonuclar)
        {
            return JsonSerializer.Serialize(sonuclar, Yazma);
        }
    }
}
