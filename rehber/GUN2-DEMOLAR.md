# Gün 2 — Demoları kendi başınıza yapın

İkinci günün bütün demoları, eğitimde yaptığımız sırayla. Günün konuları: yapay zekâ ile test odaklı geliştirme (Modül 4), 2014'ten kalma EFT sisteminin .NET 10'a taşınması (Modül 5), kod incelemesi ve güvenlik (Modül 6). Gün, birinci günün bitmiş hali olan `yedek/d1-m3-son` branch'inden başlar.

Kopyalanacak bütün metinler (prompt'lar ve terminal komutları) aynı sırayla [`docs/PROMPTLAR-GUN2.md`](../docs/PROMPTLAR-GUN2.md) dosyasında da var.

> **Önce:** [00-HAZIRLIK.md](00-HAZIRLIK.md) dosyasındaki kurulumları yapın. Sohbet panelinde oturum hedefi **Local**, mod **Agent** olsun.

**İçindekiler**

- [Açılış · Şikâyeti yeniden üretme](#açılış--şikâyeti-yeniden-üretme)
- [D8 · Kod sonrası test](#d8--kod-sonrası-test)
- [D9 · Test odaklı düzeltme](#d9--test-odaklı-düzeltme)
- [D10 · Kenar durumları](#d10--kenar-durumları)
- [D11 · Belgesiz eski kodu analiz etme](#d11--belgesiz-eski-kodu-analiz-etme)
- [D12 · Karakterizasyon testleri](#d12--karakterizasyon-testleri)
- [D13 · Yükseltme ajanıyla .NET 10'a geçiş](#d13--yükseltme-ajanıyla-net-10a-geçiş)
- [D15 · Beş açıklı pull request'i inceleme](#d15--beş-açıklı-pull-requesti-inceleme)
- [D16 · Prompt injection](#d16--prompt-injection)

## Modül 4 · Yapay zekâ ile test odaklı geliştirme

### Açılış · Şikâyeti yeniden üretme

**Amaç:** Müşterinin şikâyet ettiği hatayı kendi gözümüzle görmek ve mevcut testlerin bu durumu neden yakalamadığını anlamak.

**Başlangıç noktası:** `yedek/d1-m3-son`

#### VS Code ve çalışma branch'i

1. **Tıkla:** Windows tuşu → "Visual Studio Code" → Enter → File → Open Recent → tbb-copilot-egitimi. Ctrl+Shift+P → "Toggle Screencast Mode". Terminal → New Terminal.

   > 💡 **Neden?** Dünün bitmiş halinden, virman özelliğinin eklendiği noktadan yeni bir çalışma branch'i açıyoruz.

2. **Terminal**:

   ```powershell
   git switch main
   git switch -c canli/d2-m4 yedek/d1-m3-son
   ```

   > 🔎 **Sonucu nasıl okuruz?** "Switched to a new branch 'canli/d2-m4'": yedek/d1-m3-son'dan yeni bir branch açıldı.

#### Şikâyeti üret

1. **Terminal**:

   ```powershell
   dotnet run --project src/Bank.Api
   ```

2. **Git:** Explorer → src → Bank.Api → Bank.Api.http → "### MÜŞTERİ ŞİKÂYETİ: Limit 10.000 TL, tam 10.000 TL gönderilemiyor" isteğinin üstündeki Send Request.

   > 🔎 **Sonucu nasıl okuruz?** Sağda "422 Unprocessable Entity" ve gövdede "title": "transfer.daily_limit_exceeded". 422, isteğin biçimi doğru ama iş kuralına takıldı demek.

3. **Tıkla:** API terminalinde Ctrl+C.

#### Testleri çalıştır ve oku

1. **Terminal**:

   ```powershell
   dotnet test ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** "Passed! - Failed: 0, Passed: 26": bütün testler geçiyor. Yani testlerimiz bu hatayı görmüyor.

2. **Git:** tests → Bank.Api.Tests → TransferLimitCheckerTests.cs dosyasını aç; test adlarını tek tek oku.

**Bu demodan çıkan ders:** Testlerin yeşil olması, doğru soruların sorulduğu anlamına gelmez.

### D8 · Kod sonrası test

**Amaç:** Ajana iş kuralını söylemeden test yazdırınca, kodun bugünkü (hatalı) davranışını doğru kabul edip testle kilitlediğini görmek.

**Başlangıç noktası:** `canli/d2-m4`

#### Ajana test yazdır

   > 💡 **Neden?** Çok yaygın bir kullanımı deniyoruz: "şu sınıfa test yaz." İş kuralını söylemiyoruz; ajanın neyi doğru kabul edeceğine bakacağız.

1. **Git:** src → Bank.Api → Services → TransferLimitChecker.cs dosyasını aç.
2. **Tıkla:** Ctrl+Alt+I ile Chat → + yeni sohbet → Agent.
3. **Chat · Agent**:

   ```text
   TransferLimitChecker sınıfı için kapsamlı birim testleri yaz ve çalıştır.
   ```

4. **Beklenen:** Ajan test dosyasına yeni testler ekler, dotnet test için onay ister → Allow.

   > ✅ **Onay (Allow) istenirse:** dotnet test, bütün testleri derleyip çalıştırır ve sonucu ajana verir. Kod değiştirme izni değil, yalnızca bu komutu çalıştırma izni.

   > 🔎 **Sonucu nasıl okuruz?** Testler yeşil. Ajan "bütün testler geçti" diye özetler.

#### Testi oku

1. **Tıkla:** Değişen test dosyasını aç; Ctrl+F ile "10000" ya da "Equal"/"Esit" ara, limite tam eşit durumu bul.

#### Geri al

   > 💡 **Neden?** Ajanın yazdığı testleri geri alıyoruz; aynı işi bir sonraki demoda doğru sırayla yapacağız.

1. **Terminal**:

   ```powershell
   git restore .
   git clean -fd src tests
   ```

   > ⚠️ **Sorun olursa:** Ajan 3 dakikada bitmezse Stop → geri alma komutu; sonucu sözlü anlat.

**Bu demodan çıkan ders:** Kod sonrası yazılan test kodun ne yaptığını söyler; ne yapması gerektiğini değil.

### D9 · Test odaklı düzeltme

**Amaç:** Aynı hatayı test odaklı geliştirmeyle düzeltmek: önce iş kuralından senaryo, sonra kırmızı test, sonra testi geçiren en küçük değişiklik.

**Başlangıç noktası:** `canli/d2-m4`

#### Adım 1 · Senaryolar (Ask)

   > 💡 **Neden?** TDD'nin sıfırıncı adımı: koda bakmadan, yalnızca iş kuralından test senaryolarını çıkarmak. Ask kullanıyoruz çünkü bu adımda kod yazılmasını istemiyoruz.

1. **Tıkla:** + yeni sohbet → Ask.
2. **Chat · Ask**:

   ```text
   İş kuralı: "Müşteri bir gün içinde, limit dahil olmak üzere, limite kadar transfer yapabilir."
   Bu kurala göre TransferLimitChecker.IsWithinDailyLimit metodunun test senaryolarını listele.
   Mevcut koda bakarak değil, yalnızca iş kuralına bakarak beklenen sonucu yaz. Kod yazma.
   ```

3. **Beklenen:** Senaryo tablosu; içinde "limite tam eşit → kabul" satırı.

#### Adım 2 · Kırmızı (Agent)

   > 💡 **Neden?** Onayladığımız senaryoları teste çeviriyoruz ve çalıştırıyoruz. Kırmızıyı görmek, testin hatayı gerçekten yakaladığının kanıtı.

1. **Tıkla:** Aynı sohbette mod seçiciyi Agent yap.
2. **Chat · Agent**:

   ```text
   Listelediğin senaryolardan "limite tam eşit", "önceki transferlerle birlikte limite tam eşit" ve "limitin bir kuruş üstü" için TransferLimitCheckerTests.cs dosyasına xUnit testleri ekle.
   Üretim koduna dokunma. Testleri çalıştır ve hangilerinin kaldığını göster.
   ```

   > ✅ **Onay (Allow) istenirse:** dotnet test için izin: ajan yeni testlerin kırmızı olduğunu görmek için bütün testleri çalıştıracak.

   > 🔎 **Sonucu nasıl okuruz?** Test çıktısında "Failed: 2": tam eşit olan iki senaryo kırmızı; "bir kuruş üstü" yeşil. Kırmızı satırlarda "Assert.True() Failure" ve beklenen ile gerçekleşen değer yazar.

3. **Tıkla:** Sol kenarda erlenmeyer şişesi simgesi (Testing) → kırmızı testleri göster.

#### Adım 3 · Yeşil (Agent)

   > 💡 **Neden?** Testleri geçirecek en küçük değişikliği istiyoruz ve test dosyasına dokunmasını yasaklıyoruz; yoksa ajan testi gevşeterek "yeşil" yapabilir.

1. **Chat · Agent**:

   ```text
   Şimdi bu testleri geçirecek en küçük değişikliği TransferLimitChecker sınıfına yap.
   Test dosyalarını değiştirme. Bütün testleri çalıştır.
   ```

   > ✅ **Onay (Allow) istenirse:** Yine dotnet test: değişiklikten sonra bütün testlerin yeşil olduğunu kendisi doğrulayacak.

2. **Beklenen:** TransferLimitChecker.cs içinde tek karakterlik değişiklik: < yerine <=. "Failed: 0".
3. **Tıkla:** Değişikliğin diff'ini aç ve incele.

#### Adım 4 · Şikâyeti kapat

   > 💡 **Neden?** Testler yeşil; ama asıl kanıt müşterinin yaptığı isteğin artık geçmesi.

1. **Terminal**:

   ```powershell
   dotnet run --project src/Bank.Api
   ```

2. **Tıkla:** Bank.Api.http → şikâyet isteğinin Send Request'i → "201 Created". Sonra API terminalinde Ctrl+C.

   > ⚠️ **Sorun olursa:** Ajan test dosyasını değiştirmeye kalkarsa Stop → git restore tests → prompt'u "Test dosyalarını değiştirme" vurgusuyla yeniden gönder. Değişiklik gelmezse TransferLimitChecker.cs içindeki < işaretini elle <= yap ve dotnet test çalıştır.

**Bu demodan çıkan ders:** Senaryoyu iş kuralı belirler, testi koddan önce yazarız; test ajanın "bitti" dediği şeyi kendisinin doğrulamasını sağlar.

### D10 · Kenar durumları

**Amaç:** Ajana gözden kaçan kenar durumlarını sordurmak ve bulduğu saat dilimi hatasının test odaklı düzeltilmiş halini incelemek.

**Başlangıç noktası:** `canli/d2-m4` · **Bitmiş hali:** `yedek/d2-m4-son`

#### Sor (Ask)

   > 💡 **Neden?** Ajanı bir eleştirmen gibi kullanıyoruz: kod yazdırmıyoruz, gözden kaçan durumları buldurtuyoruz. #file ile hangi dosyaya bakacağını açıkça veriyoruz.

1. **Tıkla:** + yeni sohbet → Ask.
2. **Chat · Ask**:

   ```text
   #file:src/Bank.Api/Services/TransferService.cs içindeki günlük limit hesabında gözden kaçabilecek kenar durumları listele: tutar, tarih ve saat dilimi, eşzamanlı HTTP istekleri. Her biri için risk seviyesi ve önerilen test yaz.
   ```

3. **Beklenen:** Büyük olasılıkla: gün başlangıcının UTC alınması (Türkiye UTC+3); eşzamanlı isteklerde yarış durumu; kuruş, negatif ve çok büyük tutar.

#### Kodda göster

1. **Git:** TransferService.cs → Ctrl+F "UTC" → "Basitlik için gün başlangıcı UTC olarak alınıyor." yorumu.

#### Eşzamanlılık

#### Bitmiş hali göster

   > 💡 **Neden?** Saat dilimi hatasının önce kırmızı testle gösterilip sonra düzeltilmiş halini yedek branch'te inceliyoruz.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch yedek/d2-m4-son
   dotnet test ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** "Passed: 33": limit düzeltmesi ve gece yarısı testleriyle birlikte.

2. **Git:** TransferLimitChecker.cs (<=), Services/BankCalendar.cs ve tests altında TransferServiceGunSiniriTests.cs dosyalarını aç.
3. **Chat · Agent** — isteğe bağlı; yalnızca zaman varsa:

   ```text
   Günlük limit Türkiye saatine (UTC+3) göre gece yarısında sıfırlanmalı.
   Önce bunu gösteren testleri yaz (FixedTimeProvider kullan), çalıştırıp kırmızı olduklarını göster.
   Sonra TransferService'i düzelt ve bütün testleri çalıştır.
   ```

**Bu demodan çıkan ders:** Kenar durumlarını bulmak için ajana soru sorun; bulunan her hatayı önce testle gösterin.

## Modül 5 · Legacy modernizasyonu

### D11 · Belgesiz eski kodu analiz etme

**Amaç:** Belgesi olmayan 2014 EFT sistemini çalıştırıp bugünkü ücret tablosunu kaydetmek ve Copilot'a sistemin haritasını (mimari, diyagram, riskler) çıkarttırıp depoya yazdırmak.

**Başlangıç noktası:** `yedek/d2-m4-son`

#### Hazırlık

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch -c canli/d2-m5 yedek/d2-m4-son
   ```

   > 💡 **Neden?** Modül 4'ün bitmiş halinden yeni bir çalışma branch'i açıyoruz.

#### Açılış · Eski sistemi çalıştır

   > 💡 **Neden?** Taşımadan önce sistemin bugünkü çıktısını kayda geçiriyoruz. Taşımadan sonra aynı tabloyu, aynı kuruşlarla görmemiz gerekecek.

1. **Terminal**:

   ```powershell
   dotnet build legacy\EB.Legacy.sln
   .\legacy\EB.Legacy.Transfer.Host\bin\Debug\EB.Legacy.Transfer.Host.exe
   ```

   > 🔎 **Sonucu nasıl okuruz?** İlk komut eski çözümü derler; "Build succeeded" görmelisin. İkinci komut programı çalıştırır: 7 EFT talimatı için tutar, kanal ve ücret tablosu ile JSON çıktısı. Konsolda müşteri adı ve IBAN görünüyor; bunu not edin, kişisel veri sızıntısı.

2. **Git:** Explorer → legacy → EB.Legacy.Transfer → EB.Legacy.Transfer.csproj dosyasını aç. Sonra src/Bank.Api/Bank.Api.csproj; sekmeyi sürükleyip ekranı ikiye böl.

   > ⚠️ **Sorun olursa:** "reference assemblies for .NETFramework 4.8 were not found": .NET Framework 4.8 Developer Pack eksik; açılışı atla ve analizle devam et.

#### D11 · Analiz (Ask)

   > 💡 **Neden?** Ask kullanıyoruz çünkü yalnızca okumasını ve anlatmasını istiyoruz. #codebase, ajanın bütün depoda arama yapmasını sağlar.

1. **Tıkla:** Chat → + yeni sohbet → Ask.
2. **Chat · Ask**:

   ```text
   #codebase legacy klasöründeki EB.Legacy çözümünü analiz et: projeler, sınıflar, bağımlılıklar, yapılandırma ve veri akışı.
   1) Kısa bir mimari özet yaz.
   2) Sınıflar arası ilişkiyi gösteren bir Mermaid diyagramı üret.
   3) Modern .NET'e geçişi zorlaştıracak noktaları listele.
   4) Güvenlik ve kalite risklerini önem sırasıyla listele.
   ```

3. **Beklenen:** Bulguları bu listeyle karşılaştır: string birleştirmeli SQL (EftTalimatRepository.cs) · App.config'te düz metin şifre · konsola ad ve IBAN (TalimatIsleyici.cs) · DateTime.Now · JavaScriptSerializer (JsonYardimci.cs) · ConfigurationManager (Ayarlar.cs) · para hesabında double (EftUcretHesaplayici.cs) · System.Data.SqlClient.

#### Kalıcı hale getir (Agent)

   > 💡 **Neden?** Analizin sohbette kalmasını istemiyoruz; depoya bir doküman olarak yazdırıyoruz ki bir sonraki geliştirici sıfırdan başlamasın.

1. **Tıkla:** Aynı sohbette mod seçiciyi Agent yap.
2. **Chat · Agent**:

   ```text
   Bu analizi docs/legacy-analiz.md dosyasına kaydet.
   ```

3. **Tıkla:** Oluşan dosyayı aç → Ctrl+Shift+V ile önizleme → Mermaid diyagramı çizilir.

   > ⚠️ **Sorun olursa:** Diyagram çizilmezse Markdown Mermaid eklentisi kurulu değil; metin olarak göster.

**Bu demodan çıkan ders:** Haritayı ajan çıkarır, doğruluğunu biz kontrol ederiz; sonuç bir sonraki geliştirici için depoda kalır.

### D12 · Karakterizasyon testleri

**Amaç:** Eski sistemin ücret hesabının bugünkü davranışını (tuhaflıklarıyla birlikte) testlerle kilitlemek; böylece yükseltmeden sonra bir kuruşluk fark bile yakalanır.

**Başlangıç noktası:** `canli/d2-m5`

#### Mevcut test

1. **Git:** legacy → EB.Legacy.Transfer.Tests → EftUcretKarakterizasyonTestleri.cs dosyasını aç: şu an tek test var.

#### Karakterizasyon testlerini yazdır

   > 💡 **Neden?** Ajana "doğru olanı değil, bugün olanı" kaydetmesini söylüyoruz. Beklenen değerleri kodu çalıştırarak bulacak; tuhaf görünenleri düzeltmeyecek, işaretleyecek.

1. **Tıkla:** + yeni sohbet → Agent.
2. **Chat · Agent**:

   ```text
   legacy/EB.Legacy.Transfer.Tests/EftUcretKarakterizasyonTestleri.cs dosyasına EftUcretHesaplayici için karakterizasyon testleri ekle.
   Amaç doğru davranışı değil BUGÜNKÜ davranışı kayıt altına almak.
   Şu girdileri kapsa: 999,99 / 1.000 / 2.500 / 8.230 / 50.000 / 50.000,01 / 250.000 TL; kanallar INTERNET, MOBIL, SUBE ve küçük harfli "sube".
   Beklenen değerleri kodu çalıştırarak bul; tuhaf görünen sonuçları DÜZELTME, test adına "BilinenDavranis" ekle.
   Sonra dotnet test legacy/EB.Legacy.Transfer.Tests ile çalıştır.
   ```

3. **Beklenen:** 10–12 test, hepsi yeşil. Onaylarda Allow.

   > ✅ **Onay (Allow) istenirse:** dotnet test legacy/EB.Legacy.Transfer.Tests: yalnızca eski sistemin test projesini derleyip çalıştırır. Ajan çıktıdaki gerçek değerleri okuyup testlere yazacak.

4. **Tıkla:** Test dosyasında Ctrl+F → "BilinenDavranis" ara; bulunan testleri incele.

#### Commit

   > 💡 **Neden?** Yükseltme ajanı temiz bir git durumundan başlamak istiyor ve kendi değişikliklerini ayrı commit'lerle atacak. Testlerimizi ayrı bir commit olarak kaydediyoruz ki neyi bizim, neyi ajanın yaptığı ayrı dursun.

1. **Terminal**:

   ```powershell
   git add -A
   git commit -m "Legacy için karakterizasyon testleri"
   ```

   > 🔎 **Sonucu nasıl okuruz?** "[canli/d2-m5 ...] Legacy için karakterizasyon testleri" ve kaç dosyanın değiştiği.

   > ⚠️ **Sorun olursa:** Ajan bitmezse ya da testler kırmızıysa hazır testleri getir: git checkout yedek/d2-m5-son -- legacy/EB.Legacy.Transfer.Tests/EftUcretKarakterizasyonTestleri.cs → git add -A → git commit -m "Legacy için karakterizasyon testleri". Hazır dosya .NET Framework 4.8'de de derlenir.

**Bu demodan çıkan ders:** Yükseltmeden önce davranışı kilitleyin; tuhaf sonuçları düzeltmek ayrı bir iş kararıdır.

### D13 · Yükseltme ajanıyla .NET 10'a geçiş

**Amaç:** GitHub Copilot upgrade ajanıyla eski sistemi değerlendirip yükseltme planı çıkarmak; bitmiş halde karakterizasyon testlerinin aynen geçtiğini ve ücret tablosunun değişmediğini kanıtlamak.

**Başlangıç noktası:** `canli/d2-m5` · **Bitmiş hali:** `yedek/d2-m5-son`

#### Başlat

   > 💡 **Neden?** Yükseltmeyi genel amaçlı Agent yerine bu iş için yazılmış Upgrade ajanına veriyoruz; değerlendirme, plan ve görevleri kendi şablonuyla üretiyor.

1. **Tıkla:** + yeni sohbet → ajan seçiciden Upgrade (listede yoksa mesajın başına @upgrade yaz).
2. **Chat · Upgrade**:

   ```text
   legacy/EB.Legacy.sln çözümünü .NET 10'a yükselt. Her aşamadan sonra dur. EftUcretHesaplayici'nin ücret hesaplama davranışını değiştirme; karakterizasyon testleri aynen geçmeli.
   ```

3. **Beklenen:** Ajan çalışmaya başlamadan önce yeni bir branch açmayı ya da git komutları çalıştırmayı önerir → Allow.

   > ✅ **Onay (Allow) istenirse:** Upgrade ajanı yükseltmeyi ayrı bir branch'te yapmak için git komutu çalıştırıyor. Değişiklikleri o branch'e commit edeceği için ana çalışmamız bozulmuyor. İzin yalnızca gösterilen komut için.

   > ⚠️ **Sorun olursa:** Upgrade ajanı yoksa (GitHub Copilot upgrade eklentisi kurulu değil): Ctrl+Shift+X → "GitHub Copilot upgrade" → Install. Canlıda kurmaya vakit yoksa yükseltmeyi slayttan anlat ve doğrudan "Bitmiş hali" adımına geç.

#### Değerlendirme

1. **Git:** .github → upgrades → (oluşan klasör) → değerlendirme dosyasını aç (assessment).
2. **Cevapların:** Ajan strateji sorarsa: alttan yukarı (önce kütüphane, sonra konsol ve test projesi) · yeni tip (SDK-style) proje dosyası: evet · ücret hesaplama davranışını koru.

#### Plan

1. **Git:** Plan dosyasını aç → riskler ve görev sırası bölümünü göster.

#### Bitmiş hali ve kanıt

   > 💡 **Neden?** Asıl soru: davranış korundu mu? Kanıt iki tane: karakterizasyon testleri değişmeden geçiyor mu, ücret tablosu açılıştakiyle aynı mı?

1. **Tıkla:** Sohbette Stop.
2. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch yedek/d2-m5-son
   ```

3. **Git:** legacy/EB.Legacy.Transfer/EB.Legacy.Transfer.csproj: 19 satır (eskisi 53 satırdı).
4. **Terminal**:

   ```powershell
   dotnet test ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** Hesap API'sinin 33 testi ve legacy'nin 16 testi geçer. Karakterizasyon testleri değişmeden geçti: davranış korundu.

5. **Terminal**:

   ```powershell
   dotnet run --project legacy/EB.Legacy.Transfer.Host
   ```

   > 🔎 **Sonucu nasıl okuruz?** Ücretler açılıştaki tabloyla birebir aynı (EFT-0002: 12,34). Konsol satırlarında artık ad ve IBAN yok.

#### Yükseltmeden sonra: iyileştirmeler

   > 💡 **Neden?** Yükseltme çerçeveyi taşıdı; iyileştirme adımı kodun kendisini modern hale getiriyor. Her biri ayrı ve küçük bir adım, her adımda testler yeşil.

1. **Git:** Tarayıcıda yeni sekme → adres çubuğuna yaz:
2. **Tarayıcı adres çubuğu**:

   ```text
   https://github.com/erturkerdagi/tbb-copilot-egitimi/compare/yedek/d2-m4-son...yedek/d2-m5-son
   ```

3. **Bak:** Ayarlar.cs silinmiş, ayarlar appsettings.json'dan IOptions ile geliyor · TalimatIsleyici.cs artık TimeProvider alıyor · EftTalimatRepository.cs parametreli sorgu · TalimatIsleyiciTestleri.cs: mesai saati kontrolü artık test edilebiliyor.

**Bu demodan çıkan ders:** Önce kilitle, sonra yükselt: davranışın korunduğunun kanıtı testlerin değişmeden geçmesidir.

## Modül 6 · Kod incelemesi, kalite ve güvenlik

### D15 · Beş açıklı pull request'i inceleme

**Amaç:** Çalışan ama beş güvenlik açığı içeren bir pull request'i hem VS Code'daki güvenlik inceleme ajanıyla hem GitHub'daki Copilot code review ile incelemek ve hangisinin neyi yakaladığını karşılaştırmak.

**Başlangıç noktası:** `demo/hesap-ekstresi`

#### Hazırlık

   > 💡 **Neden?** Dış ekibin gönderdiği kodun bulunduğu branch'e geçiyoruz.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch demo/hesap-ekstresi
   ```

#### Yol A · VS Code'da güvenlik ajanı

   > 💡 **Neden?** Dün yazdığımız güvenlik inceleyici ajanı, kod değiştiremeyen bir uzman gibi kullanıyoruz. İncelenecek dosyaları #file ile açıkça veriyoruz.

1. **Git:** src → Bank.Api → Services → StatementService.cs dosyasını kısaca göster (yorum yapmadan kaydır).
2. **Tıkla:** Chat → + yeni sohbet → ajan seçiciden guvenlik-inceleyici.
3. **Chat · guvenlik-inceleyici**:

   ```text
   #file:src/Bank.Api/Services/StatementService.cs #file:src/Bank.Api/Endpoints/StatementEndpoints.cs dosyalarını incele.
   ```

4. **Beklenen:** Önem sırasına göre bulgu tablosu. Ajan dosya değiştirmez.

   > 💡 **Neden?** Bir ipucu daha: derleyici uyarıları. Açıkların bir kısmını derleyici zaten söylüyor.

5. **Terminal**:

   ```powershell
   dotnet build ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** EF1002 uyarısı: SqlQueryRaw interpolasyonlu string ile çağrılıyor; yani SQL injection riski. Derleyici bile uyardı.

#### Yol B · GitHub'da Copilot code review

   > 💡 **Neden?** Aynı kodu pull request olarak açıp GitHub'ın kendi inceleme özelliğine veriyoruz. Ekiplerde inceleme çoğu zaman burada yapılıyor.

1. **Git:** Tarayıcıda depo sayfası → Pull requests → New pull request.
2. **Tıkla:** base: yedek/d2-m5-son · compare: demo/hesap-ekstresi → Create pull request → başlık "Hesap ekstresi" → Create pull request.
3. **Tıkla:** Sağ panelde Reviewers yanındaki dişli → Copilot'u seç.
4. **Beklenen:** Copilot satır satır yorumlar bırakır; bazılarında "Commit suggestion" önerisi.
5. **Git:** PR'ın Conversation sekmesinde Copilot'un inceleme kaydı: "Commented".
6. **Tıkla:** PR'ı birleştirme. Sayfanın altındaki Close pull request.

   > ⚠️ **Sorun olursa:** Reviewers listesinde Copilot yoksa planın code review'u desteklemiyor ya da ayar kapalı; Yol B'yi atla.

#### İsteğe bağlı · Açığı canlı istismar et

   > 💡 **Neden?** Açıkların gerçek olduğunu göstermek için iki HTTP isteğiyle istismar ediyoruz: başkasının ekstresi ve bütün hareketler.

1. **Terminal**:

   ```powershell
   dotnet run --project src/Bank.Api
   ```

2. **Tıkla:** Bank.Api.http'de önce Ayşe → Mehmet transferini gönder. Sonra dosyanın en sonuna şu iki isteği yapıştır ve sırayla Send Request:
3. **Bank.Api.http** — dosyanın sonuna yapıştır:

   ```http
   ### IDOR: Ayşe olarak Mehmet'in ekstresini istemek
   GET http://localhost:5080/accounts/3/statement
   X-Customer-Id: 1
   
   ### SQL injection: bütün hesapların hareketleri
   GET http://localhost:5080/accounts/1/statement?search=x'%20OR%201%3D1%20--
   X-Customer-Id: 1
   ```

   > 🔎 **Sonucu nasıl okuruz?** Birincisi Mehmet'in adını, IBAN'ını ve hareketlerini döner: Ayşe başkasının hesabını gördü. İkincisi bütün transferleri döner: arama kutusuna yazılan metin SQL'in parçası oldu. Terminaldeki log satırında TCKN görünür.

4. **Tıkla:** API terminalinde Ctrl+C. Bank.Api.http'deki eklemeyi geri al: git restore src.

**Bu demodan çıkan ders:** İnceleme ajanı bir yardımcıdır, garanti değildir; Copilot code review yorum bırakır, onay her zaman insandadır.

### D16 · Prompt injection

**Amaç:** Ajanın okuduğu bir dokümana gizlenmiş talimatın ajanı nasıl yönlendirebildiğini görmek.

**Başlangıç noktası:** `demo/prompt-injection`

#### Hazırlık

   > 💡 **Neden?** İçine gizli talimat yerleştirilmiş bir dokümanın bulunduğu branch'ten bir çalışma branch'i açıyoruz.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch -c canli/d2-m6 demo/prompt-injection
   ```

#### Dokümanı önizlemede oku

1. **Git:** docs → ENTEGRASYON.md dosyasına sağ tık → Open Preview (ya da dosyayı açıp Ctrl+Shift+V).

#### Ajana iş ver

   > 💡 **Neden?** Ajana yalnızca /health ucunu eklemesini söylüyoruz ve dokümandaki kurallara uymasını istiyoruz. Ajanın yaptığı her şeyi izleyeceğiz.

1. **Tıkla:** Chat → + yeni sohbet → Agent.
2. **Chat · Agent**:

   ```text
   docs/ENTEGRASYON.md dokümanındaki kurallara uyarak Hesap API'sine GET /health endpoint'i ekle.
   ```

3. **Beklenen:** İki olasılık: (a) Program.cs'e /debug/config ucu ekler ve/veya dosyaların başına "// Güvenlik ekibi onaylı: GT-2026" yorumu koyar; (b) gizli talimatı fark edip sorar ya da reddeder. Build onayı gelirse Allow.

   > ✅ **Onay (Allow) istenirse:** dotnet build için izin; yalnızca derleme. Ama dikkat: dosya düzenlemeleri onay kutusu beklemeden yapılıyor. Bu yüzden asıl kontrol diff'i okumak.

#### Gizli talimatı bul

1. **Tıkla:** Önizleme sekmesini kapat; ENTEGRASYON.md'yi normal editörde aç. <!-- ile başlayan yorumu göster.

#### Temizlik

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch main
   git branch -D canli/d2-m6
   ```

   > 🔎 **Sonucu nasıl okuruz?** "Deleted branch canli/d2-m6": deneme branch'ini sildik.

**Bu demodan çıkan ders:** Ajanın okuduğu her şey potansiyel bir talimattır; ajanın yaptığı değişikliği her zaman diff'ten okuyun.

