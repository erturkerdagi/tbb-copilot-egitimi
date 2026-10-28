> Eğitim yedeği: örnek çıktı.

# Upgrade options (onaylanan kararlar)

| Karar | Seçim |
| --- | --- |
| Hedef çatı | .NET 10 (LTS) |
| Strateji | Bottom-up: önce `EB.Legacy.Transfer`, sonra Host ve Tests |
| Proje biçimi | SDK-style dönüşüm |
| Yapılandırma | `ConfigurationManager` → `IConfiguration` + `IOptions<T>` |
| JSON | `JavaScriptSerializer` → `System.Text.Json` |
| SQL istemcisi | `Microsoft.Data.SqlClient`, parametreli sorgular |
| Davranış | Ücret hesabı **değiştirilmeyecek**; karakterizasyon testleri geçmeye devam etmeli |
