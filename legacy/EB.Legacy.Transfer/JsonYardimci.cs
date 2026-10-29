using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace EB.Legacy.Transfer
{
    public static class JsonYardimci
    {
        public static List<EftTalimati> TalimatlariOku(string dosyaYolu)
        {
            string json = File.ReadAllText(dosyaYolu, Encoding.UTF8);
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Deserialize<List<EftTalimati>>(json);
        }

        public static string SonuclariYaz(List<EftSonuc> sonuclar)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(sonuclar);
        }
    }
}
