# Gün 3 — Prompt'lar ve terminal komutları

Bu dosyada gün 3 demolarında yazdığımız **her şey** kullanım sırasıyla duruyor: Copilot'a verdiğimiz prompt'lar, terminal komutları (çalışma branch'i açma, geri alma, yedeğe geçme) ve editöre yazılan satırlar. Kopyalayıp yapıştırmak için her kod kutusunun sağ üstündeki kopyalama düğmesini kullanın.

Adımların ne işe yaradığı ve beklenen sonuçlar için: [`rehber/GUN3-DEMOLAR.md`](../rehber/GUN3-DEMOLAR.md)

| Yer etiketi | Nereye yazılır |
| --- | --- |
| **Terminal** | VS Code alt paneli: Terminal → New Terminal (PowerShell) |
| **Chat · Ask / Plan / Agent** | Chat paneli (Ctrl+Alt+I) → + yeni sohbet → alttaki seçiciden ilgili mod |
| **Chat · (ajan adı)** | Ajan seçiciden o özel ajanı seçip yazın |
| **Editör / Inline chat** | Açık dosyanın içine ya da seçili koddayken Ctrl+I |

## Modül 7 · DevOps süreçlerinin otomasyonu

### D17 · CI workflow'u yazdırma ve denetleme

*Başlangıç: `yedek/d2-m5-son`*

#### Hazırlık

**Terminal**

```powershell
git switch main
git switch -c canli/d3-m7 yedek/d2-m5-son
```

#### Yazdır

**Chat · Agent**

```text
Bu depo için GitHub Actions CI workflow'u yaz (.github/workflows/ci.yml):
- Tüm pull request'lerde (hedef branch fark etmeksizin) ve main'e push'ta çalışsın.
- .NET SDK sürümünü global.json'dan alsın.
- ErdagiBank.slnx çözümünü derleyip test etsin; kapsam raporunu artifact olarak saklasın.
- permissions en az yetkiyle tanımlansın.
```

#### PR aç

**Terminal**

```powershell
git add -A
git commit -m "CI workflow"
git push -u origin canli/d3-m7
```

#### Kuralı kalıcı yap

**Terminal**

```powershell
git checkout yedek/d3-m7-son -- .github/instructions/workflows.instructions.md
```

### D18 · Dockerfile yazdırma ve denetleme

*Başlangıç: `canli/d3-m7`*

#### Yazdır

**Chat · Agent**

```text
src/Bank.Api için çok aşamalı (multi-stage) bir Dockerfile ve .dockerignore yaz.
Image root olmayan kullanıcıyla çalışsın, SQLite veritabanı dosyası /app/data altında tutulsun.
CI workflow'una image'ı derleyen (push etmeyen) bir job ekle.
```

#### Gönder ve CI'da gör

**Terminal**

```powershell
git add -A
git commit -m "Dockerfile ve image job'ı"
git push
```

**Terminal** — isteğe bağlı; Docker Desktop gerekir

```powershell
docker build -t erdagibank/hesap-api .
docker run --rm -p 8080:8080 erdagibank/hesap-api
```

### D19 · Kırık pipeline'ın teşhisi

*Başlangıç: `demo/kirik-pipeline`*

#### VS Code'da

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch demo/kirik-pipeline
dotnet build ErdagiBank.slnx
```

**Chat · Ask**

```text
#terminalLastCommand Bu derleme neden başarısız oldu? Önce kök nedeni açıkla, sonra düzeltmeyi öner. Düzeltmeyi henüz uygulama.
```

#### Düzelt ve kapat

**Terminal**

```powershell
git add -A
git commit -m "global.json düzeltildi"
git push
```

## Modül 8 · Yönetişim ve ekibe yaygınlaştırma

### D20 · Politika ve maliyet ekranları

*Başlangıç: `— (yalnızca GitHub web ekranları)`*

#### 1 · Kişisel Copilot ayarları

**Tarayıcı adres çubuğu**

```text
https://github.com/settings/copilot
```

#### 2 · Kullanım ve maliyet

**Tarayıcı adres çubuğu**

```text
https://github.com/settings/billing
```

#### 3 · Organizasyon ekranları

**Tarayıcı adres çubuğu** — <organizasyon-adı> yerine kendi organizasyonun

```text
https://github.com/organizations/<organizasyon-adı>/settings/copilot
```

## Modül 9 · Atölye: Düzenli ödeme talimatı

### Atölye 1 · Issue'lar

*Başlangıç: `yedek/d3-m7-son`*

#### Çalışma branch'i

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch main
git switch -c canli/d3-m9 yedek/d3-m7-son
```

#### Issue'ları aç

**GitHub issue açıklaması** — issue 1 · başlık: Düzenli ödeme talimatı

```text
Müşteri, her ay belirli bir günde, belirli bir hesaba, belirli tutarda tekrarlayan transfer tanımlayabilsin.

Kabul kriterleri:
- Talimat günü ayda yoksa (ör. 31 Kasım) ayın son günü kullanılır.
- Gerçekleşme tarihi hafta sonuna denk gelirse sonraki pazartesi gerçekleşir.
- Talimat normal dış transfer gibi işlenir; bakiye ve günlük limit kuralları geçerlidir.
- Bakiye yetersizse çalıştırma başarısız kaydedilir, aynı ay tekrar denenmez.
- Bir talimat aynı ay için en fazla bir kez çalışır.
- Gönderen hesap talimatı veren müşteriye ait olmalıdır.
```

**GitHub issue açıklaması** — issue 2 · başlık: Bank.Api.http isteklerine beklenen durum kodlarını ekle

```text
src/Bank.Api/Bank.Api.http dosyasındaki her isteğin üstündeki açıklama satırının sonuna, beklenen HTTP durum kodunu parantez içinde ekle. Örnek: "### Ayşe'den Mehmet'e 1.000 TL transfer (201)". Başka dosyaya dokunma.
```

### Atölye 2 · Spec Kit ile spec, plan ve görevler

*Başlangıç: `canli/d3-m9`*

#### specify

**Chat · Agent** — Adım 1'deki issue açıklamasının tamamı

```text
/speckit-specify Müşteri, her ay belirli bir günde, belirli bir hesaba, belirli tutarda tekrarlayan transfer tanımlayabilsin.

Kabul kriterleri:
- Talimat günü ayda yoksa (ör. 31 Kasım) ayın son günü kullanılır.
- Gerçekleşme tarihi hafta sonuna denk gelirse sonraki pazartesi gerçekleşir.
- Talimat normal dış transfer gibi işlenir; bakiye ve günlük limit kuralları geçerlidir.
- Bakiye yetersizse çalıştırma başarısız kaydedilir, aynı ay tekrar denenmez.
- Bir talimat aynı ay için en fazla bir kez çalışır.
- Gönderen hesap talimatı veren müşteriye ait olmalıdır.
```

#### clarify

**Chat · Agent** — aynı sohbette

```text
/speckit-clarify
```

**Chat · Agent** — tatil sorusu

```text
Resmi tatiller bu sürümde kapsam dışı; yalnızca hafta sonu kaydırması yapılır.
```

**Chat · Agent** — alıcı sorusu

```text
Alıcı hesap başka bir müşteriye ait olabilir.
```

**Chat · Agent** — iptal sorusu

```text
Talimat iptali bu sürümde yok.
```

**Chat · Agent** — çalıştırma sorusu

```text
Gün sonu çalıştırması demoda POST /payment-instructions/run?date=YYYY-MM-DD ucuyla tetiklenecek.
```

#### plan

**Chat · Agent** — aynı sohbette

```text
/speckit-plan Mevcut desenler (servis + depo + endpoint, Result<T>) kullanılacak, yeni paket yok. Transferler mevcut TransferService üzerinden yapılacak. Tarih hesabı ayrı ve saf bir ScheduleCalculator sınıfında olacak.
```

#### tasks

**Chat · Agent** — aynı sohbette

```text
/speckit-tasks
```

### Atölye 3 · Tarih kuralları önce testle

*Başlangıç: `canli/d3-m9`*

#### Kırmızı

**Chat · Agent** — aynı sohbette

```text
tasks.md'deki tarih kuralları görevleri için önce ScheduleCalculatorTests.cs testlerini yaz:
3 Kasım 2026 Salı → aynı gün; 15 Kasım 2026 Pazar → 16 Kasım; 31 → Kasım 2026'da 30 Kasım; 30 Mayıs 2026 Cumartesi → 1 Haziran; Şubat 2027'de 29 → 1 Mart 2027; 29 Şubat 2028 → aynı gün.
Testleri çalıştır, kırmızı olduklarını göster. Üretim kodu yazma.
```

#### Yeşil

**Chat · Agent**

```text
Şimdi ScheduleCalculator sınıfını yaz ve testleri yeşile çevir. Test dosyasına dokunma.
```

### Atölye 4 · Uygulama ve bitmiş hale geçiş

*Başlangıç: `canli/d3-m9`*

#### implement

**Chat · Agent** — aynı sohbette

```text
/speckit-implement
```

#### Bitmiş hale geç

**Terminal**

```powershell
git restore --staged --worktree .
git clean -fd
git switch -c canli/d3-m9-pr yedek/d3-m7-son
git checkout yedek/d3-m9-son -- .
```

**Terminal**

```powershell
dotnet test ErdagiBank.slnx
```

### Atölye 5 · İki ajanla inceleme ve çalıştırma

*Başlangıç: `canli/d3-m9-pr`*

#### teknik-lider

**Chat · teknik-lider**

```text
#file:specs/002-duzenli-odeme-talimati/spec.md ile src/Bank.Api altındaki PaymentInstruction dosyalarını karşılaştırarak incele.
```

#### guvenlik-inceleyici

**Chat · guvenlik-inceleyici**

```text
src/Bank.Api altındaki PaymentInstruction dosyalarını güvenlik açısından incele.
```

#### Çalıştır

**Terminal**

```powershell
dotnet run --project src/Bank.Api
```

### Atölye 6 · Pull request, CI ve birleştirme

*Başlangıç: `canli/d3-m9-pr`*

#### PR açıklaması

**Chat · Agent**

```text
/pr-aciklamasi
```

#### Commit ve PR

**Terminal** — #1 yerine Adım 1'deki issue numarası

```powershell
git add -A
git commit -m "Düzenli ödeme talimatı (#1)"
git push -u origin canli/d3-m9-pr
```

### Atölye 7 · Cloud agent'ın açtığı PR

*Başlangıç: `— (GitHub web)`*
