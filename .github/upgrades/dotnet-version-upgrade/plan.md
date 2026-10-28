> Eğitim yedeği: örnek çıktı.

# Plan

1. `EB.Legacy.Transfer` projesini SDK-style biçime çevir, hedefi `net10.0` yap.
2. `Ayarlar` statik sınıfını kaldır; `EftUcretAyarlari` alanlarını özelliğe çevir, `EftIslemAyarlari` ekle.
3. `EftUcretHesaplayici` ve `TalimatIsleyici` bağımlılıklarını yapıcı üzerinden al (`IOptions<T>`, `TimeProvider`).
4. `JsonYardimci`'yi `System.Text.Json`'a taşı.
5. `EftTalimatRepository`'yi `Microsoft.Data.SqlClient`'a taşı, sorguları parametreli yap.
6. Host projesini SDK-style ve `net10.0` yap; `App.config` yerine `appsettings.json`, bağlantı bilgisini ortam değişkeninden al.
7. Test projesini `net10.0`'a taşı; karakterizasyon testlerinin aynen geçtiğini doğrula.

## Riskler

| Risk | Azaltma |
| --- | --- |
| Ücret hesabında davranış değişikliği | Karakterizasyon testleri (12 test) |
| JSON tarih biçimi değişir (`\/Date()\/` → ISO 8601) | Tüketen sistemlerle teyit; host çıktısında not |
| Konsol çıktısından kişisel verinin çıkarılması | Bilinçli değişiklik, plan'da kayıtlı |
