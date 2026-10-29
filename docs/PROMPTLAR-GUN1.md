# Gün 1 — Prompt'lar ve terminal komutları

Bu dosyada gün 1 demolarında yazdığımız **her şey** kullanım sırasıyla duruyor: Copilot'a verdiğimiz prompt'lar, terminal komutları (çalışma branch'i açma, geri alma, yedeğe geçme) ve editöre yazılan satırlar. Kopyalayıp yapıştırmak için her kod kutusunun sağ üstündeki kopyalama düğmesini kullanın.

Adımların ne işe yaradığı ve beklenen sonuçlar için: [`rehber/GUN1-DEMOLAR.md`](../rehber/GUN1-DEMOLAR.md)

| Yer etiketi | Nereye yazılır |
| --- | --- |
| **Terminal** | VS Code alt paneli: Terminal → New Terminal (PowerShell) |
| **Chat · Ask / Plan / Agent** | Chat paneli (Ctrl+Alt+I) → + yeni sohbet → alttaki seçiciden ilgili mod |
| **Chat · (ajan adı)** | Ajan seçiciden o özel ajanı seçip yazın |
| **Editör / Inline chat** | Açık dosyanın içine ya da seçili koddayken Ctrl+I |

## Açılış

### Açılış · Depoyu açma ve senaryoyu çalıştırma

*Başlangıç: `main`*

#### 3 · Uygulamayı çalıştırma

**Terminal**

```powershell
dotnet run --project src/Bank.Api
```

## Modül 1 · Copilot nasıl çalışır?

### D1 · Editörde öneriler ve D2 · Ask, Plan, Agent

*Başlangıç: `main`*

#### Hazırlık · Çalışma branch'i

**Terminal**

```powershell
git switch -c canli/d1-m1 main
```

#### D1 · Yorumdan kod: tamamlama

**Editör** — yaz, sonra Enter

```csharp
// IBAN'ın TR ile başlayıp 26 karakter olduğunu kontrol eden metot
```

#### D1 · Inline chat

**Inline chat**

```text
Bu metoda XML dokümantasyon yorumu ekle; parametreleri ve dönüş değerini Türkçe açıkla.
```

**Terminal** — D1 değişikliklerini geri al

```powershell
git restore .
```

#### D2 · Ask

**Chat · Ask**

```text
Hesaptan para çekme (withdrawal) endpoint'i ekle: POST /accounts/{id}/withdrawals.
Bakiye yetersizse hata dönsün.
```

#### D2 · Plan

**Chat · Plan**

```text
Hesaptan para çekme (withdrawal) endpoint'i ekle: POST /accounts/{id}/withdrawals.
Bakiye yetersizse hata dönsün.
```

**Chat · Plan** — soru gelmezse bu adımı atla

```text
Varsayımlar uygun: yalnız bakiye güncellemesi, ayrı çekim kaydı yok. Hata kodu withdrawal.insufficient_balance olsun.
```

#### D2 · Agent

**Chat · Agent**

```text
Planı uygula.
```

**Terminal** — temizlik

```powershell
git restore .
git clean -fd src tests
```

### D3 · Aynı soru, farklı cevap ve D4 · Olmayan paket

*Başlangıç: `canli/d1-m1`*

#### D3 · Aynı soru, üç yeni sohbet

**Chat · Ask**

```text
C# ile Türk IBAN numarasını doğrulayan bir metot yaz. Sadece kodu ver.
```

#### D4 · Olmayan paket

**Chat · Ask**

```text
Projeye Microsoft.Banking.Iban NuGet paketini ekleyip IBAN doğrulamasını onunla yap. Hangi sınıf ve metodu kullanmalıyım?
```

**Tarayıcı adres çubuğu**

```text
https://www.nuget.org/packages?q=Microsoft.Banking.Iban
```

**Terminal** — modül sonu temizlik

```powershell
git restore .
git clean -fd src tests
git switch main
```

## Modül 2 · Prompt ve bağlam mühendisliği

### D5 · Kötü prompt, iyi prompt

*Başlangıç: `main`*

#### Hazırlık · Çalışma branch'i

**Terminal** — Modül 2 çalışma branch'i

```powershell
git switch -c canli/d1-m2 main
```

#### Kötü prompt

**Chat · Agent**

```text
Transfere validasyon ekle.
```

**Terminal** — kötü prompt'un değişikliklerini geri al

```powershell
git restore .
git clean -fd src tests
```

#### İyi prompt

**Chat · Agent**

```text
TransferService.TransferAsync metoduna açıklama (Description) alanı için doğrulama ekle:
- Açıklama boş olabilir, en fazla 140 karakter olabilir.
- Aşarsa ErrorType.Validation türünde "transfer.description_too_long" hatası dönsün.
Mevcut Result<T> ve TransferErrors desenini kullan, exception fırlatma.
Kural için tests/Bank.Api.Tests altında xUnit testi yaz ve dotnet test ile doğrula.
```

#### Temizlik

**Terminal** — D5 değişikliklerini geri al

```powershell
git restore src tests
git clean -fd src tests
```

### D6 · Katmanlı özelleştirme

*Başlangıç: `canli/d1-m2 (D5'te açıldı)`*

#### Hazırlık · Geri alma komutu

**Terminal** — her adımın sonundaki geri alma komutu (şimdi çalıştırma)

```powershell
git restore src tests
git clean -fd src tests
```

#### Adım 1 · Hiçbir özelleştirme yokken

**Chat · Agent**

```text
Hesaba para yatırma endpoint'i ekle: POST /accounts/{id}/deposits
```

**Terminal** — Adım 1'i geri al

```powershell
git restore src tests
git clean -fd src tests
```

#### Adım 2 · Depo talimatları: copilot-instructions.md

**Terminal** — hazır dosyayı yedek branch'ten getir

```powershell
git checkout yedek/d1-m2-son -- .github/copilot-instructions.md
```

**Chat · Agent**

```text
Hesaba para yatırma endpoint'i ekle: POST /accounts/{id}/deposits
```

**Terminal** — Adım 2'yi geri al

```powershell
git restore src tests
git clean -fd src tests
```

#### Adım 3 · Kapsamlı talimatlar: applyTo

**Terminal**

```powershell
git checkout yedek/d1-m2-son -- .github/instructions
```

#### Adım 4 · Prompt dosyası: /yeni-endpoint

**Terminal**

```powershell
git checkout yedek/d1-m2-son -- .github/prompts
```

**Chat · Agent**

```text
/yeni-endpoint Hesaba para yatırma
```

#### Adım 5 · Skill: konu gelince açılan paket

**Terminal**

```powershell
git checkout yedek/d1-m2-son -- .github/skills
```

#### Adım 6 · Özel ajan: guvenlik-inceleyici

**Terminal**

```powershell
git checkout yedek/d1-m2-son -- .github/agents
```

**Chat · guvenlik-inceleyici**

```text
#file:src/Bank.Api/Services/AccountService.cs #file:src/Bank.Api/Endpoints/AccountEndpoints.cs dosyalarındaki para yatırma kodunu incele.
```

#### Kapanış · Bitmiş hali göster

**Terminal** — temizle, bitmiş hale geç, testleri çalıştır

```powershell
git restore --staged --worktree .
git clean -fd
git switch yedek/d1-m2-son
dotnet test ErdagiBank.slnx
```

## Modül 3 · Spec-odaklı geliştirme

### D7 · Spec Kit ile virman özelliği

*Başlangıç: `yedek/d1-m2-son`*

#### Hazırlık · Temiz başlangıç

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch -c canli/d1-m3 yedek/d1-m2-son
```

#### Adım 1 · Spec Kit kurulumu

**Terminal**

```powershell
specify init --here --integration copilot --script ps --force --non-interactive
```

#### Adım 2 · Anayasa

**Chat · Agent**

```text
/speckit-constitution Erdağı Bank Hesap API'si için anayasa: (1) Önce test, değiştirilemez. (2) Müşteri adı, TCKN ve IBAN log'a ve hata mesajlarına yazılmaz. (3) Para her zaman decimal. (4) İş hataları exception değil Result<T> ile döner, HTTP'de ProblemDetails. (5) Sadelik: yeni paket veya katman ancak gerekçesiyle. Ayrıca: müşteri yalnızca kendi hesabında işlem yapar; SQL yalnızca parametreli; YZ kodu insan incelemesi olmadan main'e girmez.
```

#### Adım 3 · Spesifikasyon

**Chat · Agent**

```text
/speckit-specify Müşteri kendi hesapları arasında para aktarabilsin (virman). Bu aktarım, başka kişilere yapılan transferler için tanımlı günlük limitten bağımsız olsun.
```

#### Adım 4 · Netleştirme

**Chat · Agent** — aynı sohbette

```text
/speckit-clarify
```

**Chat · Agent** — limit sorusu

```text
Virman günlük limite tabi değil ve sonraki dış transferlerin limit hesabına da katılmaz.
```

**Chat · Agent** — başkasının hesabı sorusu

```text
Hesaplardan biri müşteriye ait değilse "Hesap bulunamadı" (404) dönsün; başkasının hesabının varlığı ele verilmesin.
```

**Chat · Agent** — para birimi sorusu

```text
Farklı para birimli hesaplar arasında virman bu sürümde yok.
```

**Chat · Agent** — ücret sorusu

```text
Virman için ücret alınmaz.
```

#### Adım 5 · Plan

**Chat · Agent** — aynı sohbette

```text
/speckit-plan Mevcut .NET 10 Minimal API, EF Core SQLite ve servis + depo + endpoint desenini kullan. Yeni paket ekleme. Virmanları mevcut Transfer tablosunda yeni bir Kind (External/Internal) alanıyla tut; günlük limit toplamı yalnızca External transferleri saysın. Uç: POST /customers/{customerId}/internal-transfers.
```

#### Adım 6 · Görevler

**Chat · Agent** — aynı sohbette

```text
/speckit-tasks
```

#### Adım 7 · Uygulama: bitmiş hale geçiş ve doğrulama

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch yedek/d1-m3-son
```

**Terminal**

```powershell
dotnet test ErdagiBank.slnx
```

**Terminal**

```powershell
dotnet run --project src/Bank.Api
```
