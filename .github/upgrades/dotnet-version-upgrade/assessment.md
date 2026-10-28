> Eğitim yedeği: Bu dosya, GitHub Copilot upgrade ajanının bu depo için ürettiği değerlendirmenin özetlenmiş bir örneğidir. Ajanın sizin makinenizde ürettiği metin farklı olabilir.

# Assessment: EB.Legacy (.NET Framework 4.8 → .NET 10)

## Kapsam

| Proje | Tür | Mevcut hedef | Proje biçimi |
| --- | --- | --- | --- |
| EB.Legacy.Transfer | Sınıf kütüphanesi | .NET Framework 4.8 | Eski tip (non-SDK) |
| EB.Legacy.Transfer.Host | Konsol uygulaması | .NET Framework 4.8 | Eski tip (non-SDK) |
| EB.Legacy.Transfer.Tests | xUnit test projesi | .NET Framework 4.8 | SDK-style |

## Kırıcı değişiklikler ve uyumsuz API'ler

| Bulgu | Konum | Etki | Öneri |
| --- | --- | --- | --- |
| `System.Web.Script.Serialization.JavaScriptSerializer` modern .NET'te yok | `JsonYardimci.cs` | Derleme hatası | `System.Text.Json` |
| `System.Configuration.ConfigurationManager` | `Ayarlar.cs` | Paket gerekir veya yapılandırma modeli değişir | `Microsoft.Extensions.Configuration` + Options |
| `System.Data.SqlClient` | `EftTalimatRepository.cs` | Eskimiş | `Microsoft.Data.SqlClient` |
| `App.config` | Host | Modern .NET'te `appsettings.json` tercih edilir | `appsettings.json` |
| `AssemblyInfo.cs` | İki proje | SDK-style projede çift öznitelik hatası | Sürüm bilgisi `.csproj`'a taşınır |

## Diğer gözlemler (yükseltme dışı)

- `EftTalimatRepository` SQL sorgularını string birleştirerek oluşturuyor (SQL injection riski).
- `App.config` içinde düz metin veritabanı şifresi var.
- `TalimatIsleyici` mesai kontrolünde `DateTime.Now` kullanıyor; test edilemez.
- Konsola alıcı adı ve IBAN yazılıyor (kişisel veri).
