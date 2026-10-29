# Gün 2 — Prompt'lar ve terminal komutları

Bu dosyada gün 2 demolarında yazdığımız **her şey** kullanım sırasıyla duruyor: Copilot'a verdiğimiz prompt'lar, terminal komutları (çalışma branch'i açma, geri alma, yedeğe geçme) ve editöre yazılan satırlar. Kopyalayıp yapıştırmak için her kod kutusunun sağ üstündeki kopyalama düğmesini kullanın.

Adımların ne işe yaradığı ve beklenen sonuçlar için: [`rehber/GUN2-DEMOLAR.md`](../rehber/GUN2-DEMOLAR.md)

| Yer etiketi | Nereye yazılır |
| --- | --- |
| **Terminal** | VS Code alt paneli: Terminal → New Terminal (PowerShell) |
| **Chat · Ask / Plan / Agent** | Chat paneli (Ctrl+Alt+I) → + yeni sohbet → alttaki seçiciden ilgili mod |
| **Chat · (ajan adı)** | Ajan seçiciden o özel ajanı seçip yazın |
| **Editör / Inline chat** | Açık dosyanın içine ya da seçili koddayken Ctrl+I |

## Modül 4 · Yapay zekâ ile test odaklı geliştirme

### Açılış · Şikâyeti yeniden üretme

*Başlangıç: `yedek/d1-m3-son`*

#### VS Code ve çalışma branch'i

**Terminal**

```powershell
git switch main
git switch -c canli/d2-m4 yedek/d1-m3-son
```

#### Şikâyeti üret

**Terminal**

```powershell
dotnet run --project src/Bank.Api
```

#### Testleri çalıştır ve oku

**Terminal**

```powershell
dotnet test ErdagiBank.slnx
```

### D8 · Kod sonrası test

*Başlangıç: `canli/d2-m4`*

#### Ajana test yazdır

**Chat · Agent**

```text
TransferLimitChecker sınıfı için kapsamlı birim testleri yaz ve çalıştır.
```

#### Geri al

**Terminal**

```powershell
git restore .
git clean -fd src tests
```

### D9 · Test odaklı düzeltme

*Başlangıç: `canli/d2-m4`*

#### Adım 1 · Senaryolar (Ask)

**Chat · Ask**

```text
İş kuralı: "Müşteri bir gün içinde, limit dahil olmak üzere, limite kadar transfer yapabilir."
Bu kurala göre TransferLimitChecker.IsWithinDailyLimit metodunun test senaryolarını listele.
Mevcut koda bakarak değil, yalnızca iş kuralına bakarak beklenen sonucu yaz. Kod yazma.
```

#### Adım 2 · Kırmızı (Agent)

**Chat · Agent**

```text
Listelediğin senaryolardan "limite tam eşit", "önceki transferlerle birlikte limite tam eşit" ve "limitin bir kuruş üstü" için TransferLimitCheckerTests.cs dosyasına xUnit testleri ekle.
Üretim koduna dokunma. Testleri çalıştır ve hangilerinin kaldığını göster.
```

#### Adım 3 · Yeşil (Agent)

**Chat · Agent**

```text
Şimdi bu testleri geçirecek en küçük değişikliği TransferLimitChecker sınıfına yap.
Test dosyalarını değiştirme. Bütün testleri çalıştır.
```

#### Adım 4 · Şikâyeti kapat

**Terminal**

```powershell
dotnet run --project src/Bank.Api
```

### D10 · Kenar durumları

*Başlangıç: `canli/d2-m4`*

#### Sor (Ask)

**Chat · Ask**

```text
#file:src/Bank.Api/Services/TransferService.cs içindeki günlük limit hesabında gözden kaçabilecek kenar durumları listele: tutar, tarih ve saat dilimi, eşzamanlı HTTP istekleri. Her biri için risk seviyesi ve önerilen test yaz.
```

#### Bitmiş hali göster

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch yedek/d2-m4-son
dotnet test ErdagiBank.slnx
```

**Chat · Agent** — isteğe bağlı; yalnızca zaman varsa

```text
Günlük limit Türkiye saatine (UTC+3) göre gece yarısında sıfırlanmalı.
Önce bunu gösteren testleri yaz (FixedTimeProvider kullan), çalıştırıp kırmızı olduklarını göster.
Sonra TransferService'i düzelt ve bütün testleri çalıştır.
```

## Modül 5 · Legacy modernizasyonu

### D11 · Belgesiz eski kodu analiz etme

*Başlangıç: `yedek/d2-m4-son`*

#### Hazırlık

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch -c canli/d2-m5 yedek/d2-m4-son
```

#### Açılış · Eski sistemi çalıştır

**Terminal**

```powershell
dotnet build legacy\EB.Legacy.sln
.\legacy\EB.Legacy.Transfer.Host\bin\Debug\EB.Legacy.Transfer.Host.exe
```

#### D11 · Analiz (Ask)

**Chat · Ask**

```text
#codebase legacy klasöründeki EB.Legacy çözümünü analiz et: projeler, sınıflar, bağımlılıklar, yapılandırma ve veri akışı.
1) Kısa bir mimari özet yaz.
2) Sınıflar arası ilişkiyi gösteren bir Mermaid diyagramı üret.
3) Modern .NET'e geçişi zorlaştıracak noktaları listele.
4) Güvenlik ve kalite risklerini önem sırasıyla listele.
```

#### Kalıcı hale getir (Agent)

**Chat · Agent**

```text
Bu analizi docs/legacy-analiz.md dosyasına kaydet.
```

### D12 · Karakterizasyon testleri

*Başlangıç: `canli/d2-m5`*

#### Karakterizasyon testlerini yazdır

**Chat · Agent**

```text
legacy/EB.Legacy.Transfer.Tests/EftUcretKarakterizasyonTestleri.cs dosyasına EftUcretHesaplayici için karakterizasyon testleri ekle.
Amaç doğru davranışı değil BUGÜNKÜ davranışı kayıt altına almak.
Şu girdileri kapsa: 999,99 / 1.000 / 2.500 / 8.230 / 50.000 / 50.000,01 / 250.000 TL; kanallar INTERNET, MOBIL, SUBE ve küçük harfli "sube".
Beklenen değerleri kodu çalıştırarak bul; tuhaf görünen sonuçları DÜZELTME, test adına "BilinenDavranis" ekle.
Sonra dotnet test legacy/EB.Legacy.Transfer.Tests ile çalıştır.
```

#### Commit

**Terminal**

```powershell
git add -A
git commit -m "Legacy için karakterizasyon testleri"
```

### D13 · Yükseltme ajanıyla .NET 10'a geçiş

*Başlangıç: `canli/d2-m5`*

#### Başlat

**Chat · Upgrade**

```text
legacy/EB.Legacy.sln çözümünü .NET 10'a yükselt. Her aşamadan sonra dur. EftUcretHesaplayici'nin ücret hesaplama davranışını değiştirme; karakterizasyon testleri aynen geçmeli.
```

#### Bitmiş hali ve kanıt

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch yedek/d2-m5-son
```

**Terminal**

```powershell
dotnet test ErdagiBank.slnx
```

**Terminal**

```powershell
dotnet run --project legacy/EB.Legacy.Transfer.Host
```

#### Yükseltmeden sonra: iyileştirmeler

**Tarayıcı adres çubuğu**

```text
https://github.com/erturkerdagi/tbb-copilot-egitimi/compare/yedek/d2-m4-son...yedek/d2-m5-son
```

## Modül 6 · Kod incelemesi, kalite ve güvenlik

### D15 · Beş açıklı pull request'i inceleme

*Başlangıç: `demo/hesap-ekstresi`*

#### Hazırlık

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch demo/hesap-ekstresi
```

#### Yol A · VS Code'da güvenlik ajanı

**Chat · guvenlik-inceleyici**

```text
#file:src/Bank.Api/Services/StatementService.cs #file:src/Bank.Api/Endpoints/StatementEndpoints.cs dosyalarını incele.
```

**Terminal**

```powershell
dotnet build ErdagiBank.slnx
```

#### İsteğe bağlı · Açığı canlı istismar et

**Terminal**

```powershell
dotnet run --project src/Bank.Api
```

**Bank.Api.http** — dosyanın sonuna yapıştır

```http
### IDOR: Ayşe olarak Mehmet'in ekstresini istemek
GET http://localhost:5080/accounts/3/statement
X-Customer-Id: 1

### SQL injection: bütün hesapların hareketleri
GET http://localhost:5080/accounts/1/statement?search=x'%20OR%201%3D1%20--
X-Customer-Id: 1
```

### D16 · Prompt injection

*Başlangıç: `demo/prompt-injection`*

#### Hazırlık

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch -c canli/d2-m6 demo/prompt-injection
```

#### Ajana iş ver

**Chat · Agent**

```text
docs/ENTEGRASYON.md dokümanındaki kurallara uyarak Hesap API'sine GET /health endpoint'i ekle.
```

#### Temizlik

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch main
git branch -D canli/d2-m6
```
