# Gün 3 — Demoları kendi başınıza yapın

Üçüncü günün bütün demoları, eğitimde yaptığımız sırayla. Günün konuları: CI/CD ve Dockerfile (Modül 7), yönetişim ekranları (Modül 8), uçtan uca atölye: düzenli ödeme talimatı (Modül 9). Gün, ikinci günün bitmiş hali olan `yedek/d2-m5-son` branch'inden başlar. GitHub adımları için deponun kendi hesabınızdaki bir kopyası (fork) gerekir: GitHub'da depo sayfasında Fork düğmesi.

Kopyalanacak bütün metinler (prompt'lar ve terminal komutları) aynı sırayla [`docs/PROMPTLAR-GUN3.md`](../docs/PROMPTLAR-GUN3.md) dosyasında da var.

> **Önce:** [00-HAZIRLIK.md](00-HAZIRLIK.md) dosyasındaki kurulumları yapın. Sohbet panelinde oturum hedefi **Local**, mod **Agent** olsun.

**İçindekiler**

- [D17 · CI workflow'u yazdırma ve denetleme](#d17--ci-workflowu-yazdırma-ve-denetleme)
- [D18 · Dockerfile yazdırma ve denetleme](#d18--dockerfile-yazdırma-ve-denetleme)
- [D19 · Kırık pipeline'ın teşhisi](#d19--kırık-pipelineın-teşhisi)
- [D20 · Politika ve maliyet ekranları](#d20--politika-ve-maliyet-ekranları)
- [Atölye 1 · Issue'lar](#atölye-1--issuelar)
- [Atölye 2 · Spec Kit ile spec, plan ve görevler](#atölye-2--spec-kit-ile-spec-plan-ve-görevler)
- [Atölye 3 · Tarih kuralları önce testle](#atölye-3--tarih-kuralları-önce-testle)
- [Atölye 4 · Uygulama ve bitmiş hale geçiş](#atölye-4--uygulama-ve-bitmiş-hale-geçiş)
- [Atölye 5 · İki ajanla inceleme ve çalıştırma](#atölye-5--iki-ajanla-inceleme-ve-çalıştırma)
- [Atölye 6 · Pull request, CI ve birleştirme](#atölye-6--pull-request-ci-ve-birleştirme)
- [Atölye 7 · Cloud agent'ın açtığı PR](#atölye-7--cloud-agentın-açtığı-pr)

## Modül 7 · DevOps süreçlerinin otomasyonu

### D17 · CI workflow'u yazdırma ve denetleme

**Amaç:** Ajana bir GitHub Actions CI workflow'u yazdırmak, altı maddelik kontrol listesiyle denetlemek ve pull request'te çalıştığını görmek.

**Başlangıç noktası:** `yedek/d2-m5-son` · **Bitmiş hali:** `yedek/d3-m7-son`

#### Hazırlık

1. **Tıkla:** Windows tuşu → "Visual Studio Code" → Enter → File → Open Recent → tbb-copilot-egitimi. Ctrl+Shift+P → "Toggle Screencast Mode". Terminal → New Terminal.

   > 💡 **Neden?** Dünün bitmiş halinden yeni bir çalışma branch'i açıyoruz. Bu branch'i birazdan GitHub'a göndereceğiz.

2. **Terminal**:

   ```powershell
   git switch main
   git switch -c canli/d3-m7 yedek/d2-m5-son
   ```

#### Yazdır

   > 💡 **Neden?** CI dosyasını sıfırdan yazmak yerine ajana gereksinimleri veriyoruz. Prompt'taki her madde, kontrol listemizdeki bir maddeye karşılık geliyor.

1. **Tıkla:** Ctrl+Alt+I → + yeni sohbet → Agent.
2. **Chat · Agent**:

   ```text
   Bu depo için GitHub Actions CI workflow'u yaz (.github/workflows/ci.yml):
   - Tüm pull request'lerde (hedef branch fark etmeksizin) ve main'e push'ta çalışsın.
   - .NET SDK sürümünü global.json'dan alsın.
   - ErdagiBank.slnx çözümünü derleyip test etsin; kapsam raporunu artifact olarak saklasın.
   - permissions en az yetkiyle tanımlansın.
   ```

3. **Beklenen:** .github/workflows/ci.yml oluşur.

#### Denetle

   > 💡 **Neden?** Ajanın yazdığı dosyayı kontrol listesiyle tek tek denetliyoruz. Çalışan bir dosya ile güvenli bir dosya aynı şey değil.

1. **Git:** ci.yml dosyasını aç. Kontrol listesini sırayla kontrol et: on: (pull_request, push main) · permissions: contents: read · actions/setup-dotnet + global-json-file · restore/build/test · upload-artifact · sır yok · @v4 gibi sabit sürümler.

#### PR aç

   > 💡 **Neden?** CI'ın gerçekten çalıştığını görmenin tek yolu GitHub'a göndermek. Branch'i gönderip pull request açıyoruz.

1. **Terminal**:

   ```powershell
   git add -A
   git commit -m "CI workflow"
   git push -u origin canli/d3-m7
   ```

   > 🔎 **Sonucu nasıl okuruz?** push çıktısının sonunda GitHub "Create a pull request for 'canli/d3-m7'" diye bir adres yazar; branch GitHub'a gitti.

2. **Git:** Tarayıcıda depo sayfası → sarı "canli/d3-m7 had recent pushes" bandında Compare & pull request → base: yedek/d2-m5-son → Create pull request.
3. **Beklenen:** PR sayfasının altında "Some checks haven't completed yet", birkaç dakika sonra yeşil tik. Details → adım adım log.

#### Kuralı kalıcı yap

   > 💡 **Neden?** Bir sonraki workflow'un baştan doğru üretilmesi için kontrol listemizi talimat dosyasına yazıyoruz; birinci gündeki katmanlar burada da çalışıyor.

1. **Terminal**:

   ```powershell
   git checkout yedek/d3-m7-son -- .github/instructions/workflows.instructions.md
   ```

2. **Git:** .github → instructions → workflows.instructions.md dosyasını aç; applyTo satırını incele: .github/workflows altındaki YAML dosyaları.

   > ⚠️ **Sorun olursa:** Ajanın workflow'u çalışmazsa: git checkout yedek/d3-m7-son -- .github/workflows/ci.yml → git add -A → git commit -m "CI workflow" → git push.

**Bu demodan çıkan ders:** Ajanın yazdığı pipeline da kod incelemesinden geçer; en sık atlanan iki ayar permissions ve sürüm sabitlemedir.

### D18 · Dockerfile yazdırma ve denetleme

**Amaç:** Hesap API'si için çok aşamalı bir Dockerfile yazdırmak, sürüm halüsinasyonu (port) ve güvenlik (root kullanıcı) açısından denetlemek ve CI'a image derleme adımı eklemek.

**Başlangıç noktası:** `canli/d3-m7` · **Bitmiş hali:** `yedek/d3-m7-son`

#### Yazdır

   > 💡 **Neden?** Container tarafını da ajana yazdırıyoruz ama kısıtları biz veriyoruz: root olmayan kullanıcı ve verinin nerede tutulacağı.

1. **Tıkla:** Chat → + yeni sohbet → Agent.
2. **Chat · Agent**:

   ```text
   src/Bank.Api için çok aşamalı (multi-stage) bir Dockerfile ve .dockerignore yaz.
   Image root olmayan kullanıcıyla çalışsın, SQLite veritabanı dosyası /app/data altında tutulsun.
   CI workflow'una image'ı derleyen (push etmeyen) bir job ekle.
   ```

3. **Beklenen:** Depo kökünde Dockerfile ve .dockerignore; ci.yml'de ikinci bir job.

#### Denetle

1. **Git:** Dockerfile'ı aç ve kontrol listesini göster: FROM ...sdk:10.0 AS build ve FROM ...aspnet:10.0 · önce COPY *.csproj + dotnet restore · EXPOSE 8080 · USER $APP_UID. Sonra .dockerignore.

#### Gönder ve CI'da gör

1. **Terminal**:

   ```powershell
   git add -A
   git commit -m "Dockerfile ve image job'ı"
   git push
   ```

2. **Git:** Tarayıcıda PR sayfasını yenile → Checks: ikinci job çalışıyor.
3. **Terminal** — isteğe bağlı; Docker Desktop gerekir:

   ```powershell
   docker build -t erdagibank/hesap-api .
   docker run --rm -p 8080:8080 erdagibank/hesap-api
   ```

   > ⚠️ **Sorun olursa:** Ajanın Dockerfile'ı CI'da kırılırsa: git checkout yedek/d3-m7-son -- Dockerfile .dockerignore .github/workflows/ci.yml → commit → push.

**Bu demodan çıkan ders:** Ajanın eski bilgiyle yazdığı ayrıntıları (ör. port 80) ancak kontrol listesiyle yakalarız.

### D19 · Kırık pipeline'ın teşhisi

**Amaç:** Kırılan bir pipeline'da ajana önce "neden" sorup kök nedeni bulmak, düzeltmeyi kendimiz onaylayıp CI'ın yeşile döndüğünü görmek.

**Başlangıç noktası:** `demo/kirik-pipeline`

#### GitHub'da

   > 💡 **Neden?** Hatayı önce olduğu yerde, CI logunda görüyoruz.

1. **Git:** Tarayıcıda depo → Pull requests → New pull request → base: yedek/d3-m7-son · compare: demo/kirik-pipeline → Create pull request → Create pull request.
2. **Beklenen:** Kırmızı çarpı. Details → kırmızı adım ".NET SDK kur (global.json'daki sürüm)".
3. **Tıkla:** Log sayfasında Copilot'un "Explain error" düğmesi varsa tıkla ve açıklamayı göster.

#### VS Code'da

   > 💡 **Neden?** Aynı hatayı kendi makinemizde üretiyoruz; böylece ajana tam terminal çıktısını bağlam olarak verebiliriz.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch demo/kirik-pipeline
   dotnet build ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** "A compatible .NET SDK was not found" ve "Requested SDK version: 10.0.999". global.json var olmayan bir SDK sürümü istiyor.

2. **Tıkla:** Chat → + yeni sohbet → Ask.

   > 💡 **Neden?** Ask kullanıyoruz ve "düzeltmeyi henüz uygulama" diyoruz: önce teşhis. #terminalLastCommand son komutu ve çıktısını bağlama ekler.

3. **Chat · Ask**:

   ```text
   #terminalLastCommand Bu derleme neden başarısız oldu? Önce kök nedeni açıkla, sonra düzeltmeyi öner. Düzeltmeyi henüz uygulama.
   ```

4. **Beklenen:** global.json'ın var olmayan 10.0.999 sürümünü ve rollForward: disable ayarını istediğini açıklar; 10.0.100 + latestFeature önerir.

#### Düzelt ve kapat

1. **Git:** global.json dosyasını aç; "version": "10.0.100" ve "rollForward": "latestFeature" yap, kaydet (Ctrl+S).
2. **Terminal**:

   ```powershell
   git add -A
   git commit -m "global.json düzeltildi"
   git push
   ```

3. **Beklenen:** PR sayfasında CI yeniden çalışır ve yeşile döner.

**Bu demodan çıkan ders:** Kırık bir pipeline'da ajana önce kök nedeni sorun; "düzelt" demek semptomu gizleyebilir.

## Modül 8 · Yönetişim ve ekibe yaygınlaştırma

### D20 · Politika ve maliyet ekranları

**Amaç:** Yönetim kurulunun sorularına karşılık gelen GitHub ayar ekranlarını görmek: public kod eşleşmesi, kullanım ve maliyet, organizasyon politikaları, denetim kaydı ve depo kuralları.

**Başlangıç noktası:** `— (yalnızca GitHub web ekranları)`

#### 1 · Kişisel Copilot ayarları

   > 💡 **Neden?** Lisans riskini yöneten ayar burada: public kodla eşleşen öneriler.

1. **Git:** Tarayıcıda yeni sekme (Ctrl+T) → adres çubuğuna yaz:
2. **Tarayıcı adres çubuğu**:

   ```text
   https://github.com/settings/copilot
   ```

3. **Bak:** Hangi plandasın · public kodla eşleşen öneriler: Block / Allow · etkin modeller listesi.

#### 2 · Kullanım ve maliyet

1. **Tarayıcı adres çubuğu**:

   ```text
   https://github.com/settings/billing
   ```

2. **Bak:** Kullanım (usage) bölümü → Copilot filtresi: AI Credit tüketimi ve hangi özelliğin ne kadar harcadığı.

#### 3 · Organizasyon ekranları

1. **Tarayıcı adres çubuğu** — <organizasyon-adı> yerine kendi organizasyonun:

   ```text
   https://github.com/organizations/<organizasyon-adı>/settings/copilot
   ```

2. **Bak:** Access: lisans kime atanmış · Policies: agent, cloud agent, code review, CLI aç/kapa · Models · Custom instructions · MCP · Content exclusion · Organizasyon ayarları → Audit log → "copilot" filtresi.

#### 4 · Depo kuralları

1. **Git:** Depo sayfası → Settings → Rules → Rulesets → New ruleset → New branch ruleset.
2. **Bak:** Kaydetmeden: Require a pull request before merging · Require status checks to pass · Automatically request Copilot code review.

**Bu demodan çıkan ders:** Konuştuğumuz her riskin GitHub'da bir ayarı var; yönetişim yeni bir süreç değil, mevcut süreçleri yapay zekâya da uygulamaktır.

## Modül 9 · Atölye: Düzenli ödeme talimatı

### Atölye 1 · Issue'lar

**Amaç:** Büyük ve belirsiz işi (düzenli ödeme talimatı) kendimize, küçük ve net işi Copilot cloud agent'a issue olarak vermek.

**Başlangıç noktası:** `yedek/d3-m7-son`

#### Çalışma branch'i

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch main
   git switch -c canli/d3-m9 yedek/d3-m7-son
   ```

   > 💡 **Neden?** Atölyeyi Modül 7'nin bitmiş halinden, CI'ı olan bir depodan başlatıyoruz.

#### Issue'ları aç

   > 💡 **Neden?** İşi issue olarak tanımlıyoruz; hem bizim için iş takibi, hem cloud agent için girdi.

1. **Git:** Tarayıcıda depo → Issues → New issue. Başlık: "Düzenli ödeme talimatı". Açıklama: PROMPTLAR-GUN3.md'deki issue metni → Create.
2. **GitHub issue açıklaması** — issue 1 · başlık: Düzenli ödeme talimatı:

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

3. **Not al:** Issue numarasını (ör. #1) not et; Adım 6'da commit mesajına yazacaksın.
4. **Git:** Issues → New issue → Başlık: "Bank.Api.http isteklerine beklenen durum kodlarını ekle". Açıklama aşağıdaki metin → Create.
5. **GitHub issue açıklaması** — issue 2 · başlık: Bank.Api.http isteklerine beklenen durum kodlarını ekle:

   ```text
   src/Bank.Api/Bank.Api.http dosyasındaki her isteğin üstündeki açıklama satırının sonuna, beklenen HTTP durum kodunu parantez içinde ekle. Örnek: "### Ayşe'den Mehmet'e 1.000 TL transfer (201)". Başka dosyaya dokunma.
   ```

6. **Tıkla:** İkinci issue'da sağ panel → Assignees → Copilot.

   > 🔎 **Sonucu nasıl okuruz?** Issue'da Copilot'un işe başladığını gösteren bir olay; birkaç dakika içinde copilot/... adında bir branch ve taslak PR.

   > ⚠️ **Sorun olursa:** Copilot Assignees listesinde yoksa cloud agent planında yok ya da depoda kapalı; Adım 7'yi atla ve kavramı sözlü anlat.

**Bu demodan çıkan ders:** İyi yazılmış bir issue küçük bir spec'tir; küçük ve net işleri arka planda çalışan ajana verebiliriz.

### Atölye 2 · Spec Kit ile spec, plan ve görevler

**Amaç:** Issue'daki talebi birinci gündeki Spec Kit akışıyla spec'e, netleştirilmiş kararlara, teknik plana ve görev listesine dönüştürmek.

**Başlangıç noktası:** `canli/d3-m9`

#### specify

   > 💡 **Neden?** Issue metnini doğrudan spec'e dönüştürüyoruz. Spec Kit bu depoda birinci günden beri kurulu.

1. **Tıkla:** VS Code → Chat → + yeni sohbet → Agent.
2. **Chat · Agent** — Adım 1'deki issue açıklamasının tamamı:

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

   > ✅ **Onay (Allow) istenirse:** create-new-feature.ps1 betiği specs altında 002 numaralı yeni bir klasör açar ve spec şablonunu kopyalar.

3. **Git:** specs/002-.../spec.md'yi aç; kabul kriterlerinin Given/When/Then senaryolarına dönüştüğünü göster.

#### clarify

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-clarify
   ```

2. **Cevapların:** Sorunun konusuna göre:
3. **Chat · Agent** — tatil sorusu:

   ```text
   Resmi tatiller bu sürümde kapsam dışı; yalnızca hafta sonu kaydırması yapılır.
   ```

4. **Chat · Agent** — alıcı sorusu:

   ```text
   Alıcı hesap başka bir müşteriye ait olabilir.
   ```

5. **Chat · Agent** — iptal sorusu:

   ```text
   Talimat iptali bu sürümde yok.
   ```

6. **Chat · Agent** — çalıştırma sorusu:

   ```text
   Gün sonu çalıştırması demoda POST /payment-instructions/run?date=YYYY-MM-DD ucuyla tetiklenecek.
   ```

7. **Diğer:** Listede olmayan soruda ajanın önerisini kabul et.

#### plan

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-plan Mevcut desenler (servis + depo + endpoint, Result<T>) kullanılacak, yeni paket yok. Transferler mevcut TransferService üzerinden yapılacak. Tarih hesabı ayrı ve saf bir ScheduleCalculator sınıfında olacak.
   ```

2. **Git:** plan.md → Constitution Check bölümünü göster.

#### tasks

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-tasks
   ```

2. **Git:** tasks.md → tarih kurallarının ayrı bir aşama olduğunu ve testlerin önde geldiğini göster.

   > ⚠️ **Sorun olursa:** Bir adım 5 dakikayı aşarsa Stop ve yedekteki dosyalarla devam et: git checkout yedek/d3-m9-son -- specs/002-duzenli-odeme-talimati

**Bu demodan çıkan ders:** Talep büyüdükçe ve belirsizleştikçe spec'in değeri artar.

### Atölye 3 · Tarih kuralları önce testle

**Amaç:** Düzenli ödemenin tarih kurallarını (ayda olmayan gün, hafta sonu, artık yıl) önce testlerle yazıp kırmızıyı görmek, sonra kodu yazdırıp yeşile çevirmek.

**Başlangıç noktası:** `canli/d3-m9`

#### Kırmızı

   > 💡 **Neden?** Spec'teki kuralları somut tarihlere çeviriyoruz. Her tarih bir müşteri şikâyeti olabilirdi.

1. **Chat · Agent** — aynı sohbette:

   ```text
   tasks.md'deki tarih kuralları görevleri için önce ScheduleCalculatorTests.cs testlerini yaz:
   3 Kasım 2026 Salı → aynı gün; 15 Kasım 2026 Pazar → 16 Kasım; 31 → Kasım 2026'da 30 Kasım; 30 Mayıs 2026 Cumartesi → 1 Haziran; Şubat 2027'de 29 → 1 Mart 2027; 29 Şubat 2028 → aynı gün.
   Testleri çalıştır, kırmızı olduklarını göster. Üretim kodu yazma.
   ```

   > ✅ **Onay (Allow) istenirse:** dotnet test; ScheduleCalculator sınıfı henüz olmadığı için derleme hatası ya da kırmızı test bekliyoruz. Bu beklenen "kırmızı".

   > 🔎 **Sonucu nasıl okuruz?** "error CS0246: The type or namespace name 'ScheduleCalculator' could not be found" ya da kırmızı testler: sınıf henüz yok.

#### Yeşil

1. **Chat · Agent**:

   ```text
   Şimdi ScheduleCalculator sınıfını yaz ve testleri yeşile çevir. Test dosyasına dokunma.
   ```

   > ✅ **Onay (Allow) istenirse:** dotnet test: bütün testlerin yeşil olduğunu kendisi doğrulayacak.

2. **Beklenen:** ScheduleCalculator.cs ve bütün testler yeşil. Testing panelinde göster.

#### Kontrol

   > ⚠️ **Sorun olursa:** Bir test beklenmedik şekilde kırmızı kalırsa beklenen değeri birlikte iş kuralından kontrol et; ajan test dosyasını değiştirmeye kalkarsa Stop → git restore tests.

**Bu demodan çıkan ders:** Tarih kuralları sessiz mantık hatalarının saklandığı yerlerdir; somut tarihlerle yazılmış testler bu hataları kod yazılmadan yakalar.

### Atölye 4 · Uygulama ve bitmiş hale geçiş

**Amaç:** Görev listesinin uygulanmaya başladığını görmek, sonra bitmiş özelliği commit edilmemiş değişiklik olarak getirip testlerle doğrulamak.

**Başlangıç noktası:** `canli/d3-m9`

#### implement

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-implement
   ```

   > ✅ **Onay (Allow) istenirse:** Uygulama sırasında dotnet build ve dotnet test için onay ister; her biri yalnızca o komut için.

2. **Tıkla:** 2 dakika sonra Stop.

#### Bitmiş hale geç

   > 💡 **Neden?** Bitmiş hali commit edilmemiş değişiklik olarak getiriyoruz; böylece bir sonraki adımda ajanlara inceletebiliyoruz.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch -c canli/d3-m9-pr yedek/d3-m7-son
   git checkout yedek/d3-m9-son -- .
   ```

   > 🔎 **Sonucu nasıl okuruz?** Son komut, yedek/d3-m9-son branch'indeki bütün dosyaları bulunduğumuz branch'e kopyalar; branch değiştirmez. Sol kenarda Source Control simgesinde 20 dosya (16 yeni, 4 değişen).

2. **Terminal**:

   ```powershell
   dotnet test ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** Hesap API'sinin 52 testi ve legacy'nin 16 testi geçer.

**Bu demodan çıkan ders:** Bitmiş hali "değişiklik" olarak getirmek, sonraki adımda ajanlara inceletmeyi mümkün kılar.

### Atölye 5 · İki ajanla inceleme ve çalıştırma

**Amaç:** Değişiklikleri spec'e göre teknik lider ajanına, güvenlik açısından güvenlik ajanına inceletmek ve özelliği HTTP istekleriyle çalıştırmak.

**Başlangıç noktası:** `canli/d3-m9-pr`

#### teknik-lider

   > 💡 **Neden?** Teknik lider ajanı kodun spec'teki her gereksinimi karşılayıp karşılamadığına bakar. Bu ajan da yalnızca okuyabilir.

1. **Tıkla:** Chat → + yeni sohbet → ajan seçiciden teknik-lider.
2. **Chat · teknik-lider**:

   ```text
   #file:specs/002-duzenli-odeme-talimati/spec.md ile src/Bank.Api altındaki PaymentInstruction dosyalarını karşılaştırarak incele.
   ```

3. **Beklenen:** FR-001 … FR-010 için karşılama tablosu ve bir karar.

   > ⚠️ **Sorun olursa:** Ajan dosyaları bulamazsa tek tek ver: #file:src/Bank.Api/Services/PaymentInstructionService.cs #file:src/Bank.Api/Endpoints/PaymentInstructionEndpoints.cs

#### guvenlik-inceleyici

1. **Tıkla:** + yeni sohbet → guvenlik-inceleyici.
2. **Chat · guvenlik-inceleyici**:

   ```text
   src/Bank.Api altındaki PaymentInstruction dosyalarını güvenlik açısından incele.
   ```

3. **Beklenen:** Bulguları kısaca göster. [Gün sonu ucunun dışarıya açık olduğu gibi bir bulgu çıktıysa plan.md'deki risk tablosuyla karşılaştır.]

#### Çalıştır

1. **Terminal**:

   ```powershell
   dotnet run --project src/Bank.Api
   ```

2. **Tıkla:** Bank.Api.http → ATÖLYE başlıklı HTTP isteklerini sırayla Send Request: kira talimatı (ayın 5'i) → 201 · aidat talimatı (ayın 31'i) → 201 · 5 Kasım 2026 çalıştırması → "succeeded": 1 · aynı gün tekrar çalıştırma → "runs": [].

   > 🔎 **Sonucu nasıl okuruz?** 5 Kasım'da yalnızca kira talimatının vadesi gelmiş. Aynı gün ikinci çalıştırmada transfer yapılmıyor: "bir talimat ayda bir kez" kuralı çalışıyor.

3. **Tıkla:** API terminalinde Ctrl+C.

**Bu demodan çıkan ders:** Farklı rollerdeki ajanlar farklı soruları sorar; son kararı yine insan verir.

### Atölye 6 · Pull request, CI ve birleştirme

**Amaç:** Prompt dosyasıyla PR açıklaması yazdırmak, değişikliği issue numarasıyla commit edip PR açmak, Copilot incelemesini ve CI'ı görüp birleştirmek.

**Başlangıç noktası:** `canli/d3-m9-pr`

#### PR açıklaması

   > 💡 **Neden?** PR açıklamasını kurumun şablonuna göre yazan bir prompt dosyası var: /pr-aciklamasi. Değişiklikleri okuyup neden, ne, nasıl test edildi bölümlerini doldurur.

1. **Tıkla:** Chat → + yeni sohbet (Agent).
2. **Chat · Agent**:

   ```text
   /pr-aciklamasi
   ```

3. **Beklenen:** PR açıklaması taslağı. "Neden" kısmını sorarsa issue numarasını ver. Metni kopyala.

#### Commit ve PR

1. **Terminal** — #1 yerine Adım 1'deki issue numarası:

   ```powershell
   git add -A
   git commit -m "Düzenli ödeme talimatı (#1)"
   git push -u origin canli/d3-m9-pr
   ```

   > 🔎 **Sonucu nasıl okuruz?** Commit mesajındaki (#1) GitHub'da issue'yla otomatik bağlantı kurar.

2. **Git:** Tarayıcıda depo → Compare & pull request → base: atolye-baz → açıklamayı yapıştır → Create pull request.
3. **Tıkla:** Reviewers → dişli → Copilot.

#### CI, inceleme ve birleştirme

1. **Beklenen:** CI yeşile döner; Copilot yorumlar bırakır. Yorumları birlikte oku: hangisi değerli, hangisi gereksiz?
2. **Tıkla:** Merge pull request → Confirm merge.

   > ⚠️ **Sorun olursa:** CI uzun sürerse birleştirmeyi beklemeden Adım 7'ye geç, sonra dön.

**Bu demodan çıkan ders:** Yapay zekânın yazdığı kod da aynı yoldan geçer: PR, inceleme, CI ve insan onayı.

### Atölye 7 · Cloud agent'ın açtığı PR

**Amaç:** Atölyenin başında Copilot'a atadığımız issue için cloud agent'ın arka planda açtığı pull request'i incelemek.

**Başlangıç noktası:** `— (GitHub web)`

#### PR'ı incele

1. **Git:** Tarayıcıda depo → Pull requests → Copilot'un açtığı PR.
2. **Bak:** Branch adı copilot/... · commit'lerin yazarı Copilot, ortak yazar sen · değişiklik yalnızca Bank.Api.http dosyasında.
3. **Tıkla:** Files changed → diff'i birlikte oku.

**Bu demodan çıkan ders:** Cloud agent yalnızca copilot/ ile başlayan branch'lere yazar ve kendi PR'ını onaylayamaz; birleştirme kararı insandadır.

