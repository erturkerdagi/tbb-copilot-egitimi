> Eğitim yedeği: örnek çıktı.

# Tasks

| # | Görev | Doğrulama | Durum |
| --- | --- | --- | --- |
| 1 | `EB.Legacy.Transfer` → SDK-style, net10.0 | `dotnet build` | ✅ |
| 2 | `Ayarlar` kaldırıldı, Options modeli | `dotnet build` | ✅ |
| 3 | `TimeProvider` ve yapıcı enjeksiyonu | `dotnet build` | ✅ |
| 4 | `System.Text.Json` | Host çalıştırıldı | ✅ |
| 5 | `Microsoft.Data.SqlClient`, parametreli sorgu | `dotnet build` | ✅ |
| 6 | Host → net10.0, `appsettings.json` | Host çıktısı eski sürümle karşılaştırıldı | ✅ |
| 7 | Tests → net10.0 | `dotnet test`: tümü geçti | ✅ |
