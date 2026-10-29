# Gün 1 — Demoları kendi başınıza yapın

Birinci günün bütün demoları, eğitimde yaptığımız sırayla. Her demonun başında amacı ve hangi branch'ten başlandığı yazıyor; adımların altında "Neden?" notları ve sonuçların nasıl okunacağı var. Günün konuları: Copilot'un çalışma biçimi ve modları (Modül 1), kurumun kurallarını Copilot'a öğreten dosyalar (Modül 2), Spec Kit ile spec-odaklı geliştirme (Modül 3).

Kopyalanacak bütün metinler (prompt'lar ve terminal komutları) aynı sırayla [`docs/PROMPTLAR-GUN1.md`](../docs/PROMPTLAR-GUN1.md) dosyasında da var.

> **Önce:** [00-HAZIRLIK.md](00-HAZIRLIK.md) dosyasındaki kurulumları yapın. Sohbet panelinde oturum hedefi **Local**, mod **Agent** olsun.

**İçindekiler**

- [Açılış · Depoyu açma ve senaryoyu çalıştırma](#açılış--depoyu-açma-ve-senaryoyu-çalıştırma)
- [D1 · Editörde öneriler ve D2 · Ask, Plan, Agent](#d1--editörde-öneriler-ve-d2--ask-plan-agent)
- [D3 · Aynı soru, farklı cevap ve D4 · Olmayan paket](#d3--aynı-soru-farklı-cevap-ve-d4--olmayan-paket)
- [D5 · Kötü prompt, iyi prompt](#d5--kötü-prompt-iyi-prompt)
- [D6 · Katmanlı özelleştirme](#d6--katmanlı-özelleştirme)
- [D7 · Spec Kit ile virman özelliği](#d7--spec-kit-ile-virman-özelliği)

## Açılış

### Açılış · Depoyu açma ve senaryoyu çalıştırma

**Amaç:** Demo deposunu VS Code'da açmak, ekranın bölümlerini tanımak ve Hesap API'sini çalıştırıp ilk HTTP isteklerini göndermek.

**Başlangıç noktası:** `main`

#### 1 · VS Code'u ve depoyu açma

   > 💡 **Neden?** Copilot, VS Code'da açtığımız klasörü proje olarak görür ve bağlamını oradan toplar. Doğru klasörü açmak, Copilot'a doğru projeyi göstermek demek.

1. **Tıkla:** Windows tuşu → "Visual Studio Code" yaz → Enter.
2. **Tıkla:** Ctrl+Shift+P → açılan kutuya "Toggle Screencast Mode" yaz → Enter. Bastığın tuşlar artık ekranın altında görünür.
3. **Tıkla:** File → Open Folder... → C:\egitim\tbb-copilot-egitimi klasörü → "Klasör Seç".
4. **Beklenen:** "Do you trust the authors of the files in this folder?" sorusu.
5. **Tıkla:** "Yes, I trust the authors".

   > ⚠️ **Sorun olursa:** Klasörü kısıtlı modda (Restricted Mode) açtıysan Copilot ajanı dosya düzenleyemez ve komut çalıştıramaz. En alttaki "Restricted Mode" yazısına tıkla → Trust.

6. **Beklenen:** Sol altta branch adı: main. C# Dev Kit projeyi yüklerken 10–20 saniye bir yükleme göstergesi görünür.

#### 2 · Ekranı tanıma

1. **Git:** Sol kenardaki Explorer (Ctrl+Shift+E). Klasörlerin üzerinden göz gezdir.
2. **Tıkla:** Ctrl+Alt+I ile Chat panelini aç. Mesaj kutusunun altındaki satırı göster.

   > 💡 **Neden?** Chat panelinin altındaki satırda iki seçici var: oturum hedefi ve mod. Oturum hedefini "Local" yapıyoruz, çünkü prompt dosyaları, Plan modu ve kendi yazacağımız ajanlar yalnızca Local oturumda çalışıyor.

3. **Tıkla:** En alttaki oturum hedefi "Copilot" yazıyorsa ona tıkla → Local. Mod seçicide Agent seçili olsun. Model seçicide günün modelini seç.

#### 3 · Uygulamayı çalıştırma

   > 💡 **Neden?** Bir değişikliği değerlendirebilmek için önce sistemin bugünkü halini çalışırken görmemiz gerekiyor.

1. **Tıkla:** Terminal → New Terminal. Alt panelde PowerShell açılır ve depo klasöründe başlar.
2. **Terminal**:

   ```powershell
   dotnet run --project src/Bank.Api
   ```

   > 🔎 **Sonucu nasıl okuruz?** Önce "Building..." ile derleme, sonra "Now listening on: http://localhost:5080" satırı: API bilgisayarımızda 5080 portunda çalışıyor. "Application started. Press Ctrl+C to shut down." terminalin artık API'ye ait olduğunu söylüyor; bu terminale komut yazmıyoruz. Sarı NU1903 uyarısı görürsen bu bir paket güvenlik uyarısı; derlemeyi engellemiyor, ikinci gün konuşacağız.

#### 4 · API'ye HTTP isteği gönderme

1. **Git:** Explorer → src → Bank.Api → Bank.Api.http dosyasına tıkla.
2. **Tıkla:** "### Ayşe'nin vadesiz hesabı (Id = 1)" başlığının altındaki GET satırının üstünde, gri renkte yazan Send Request.

   > 🔎 **Sonucu nasıl okuruz?** Sağda yanıt sekmesi açılır. İlk satırdaki "HTTP/1.1 200 OK" isteğin başarılı olduğunu söyler; altındaki JSON hesabın bilgileri: sahibi Ayşe, bakiye 25.000.

3. **Tıkla:** "### Ayşe'den Mehmet'e 1.000 TL transfer" isteğinin üstündeki Send Request.

   > 🔎 **Sonucu nasıl okuruz?** "201 Created": yeni bir kayıt oluştu. Gövdede transferin numarası, tutarı ve tarihi var.

#### 5 · Veritabanına bakma ve kapatma

1. **Git:** Explorer'da depo kökünde oluşan erdagibank.db dosyasına tıkla (SQLite Viewer açılır) → Accounts tablosu: Ayşe'nin 1 numaralı hesabında 24.000 → Transfers: az önceki 1.000 TL'lik satır.
2. **Tıkla:** Terminal paneline tıkla → Ctrl+C ile API'yi durdur. Açık sekmeleri kapat (Ctrl+K, ardından W). VS Code açık kalsın.

   > ⚠️ **Sorun olursa:** "dotnet bulunamadı": .NET 10 SDK kurulu değil ya da kurulumdan sonra VS Code yeniden açılmamış. "Send Request" yazısı yok: REST Client eklentisi kurulu değil (Ctrl+Shift+X → "REST Client" → Install). Port meşgul hatası: önceki bir çalıştırma açık kalmış; o terminalde Ctrl+C.

**Bu demodan çıkan ders:** Copilot bağlamını VS Code'da açık olan klasörden alır; doğru klasörü açmak, Copilot'a doğru projeyi göstermek demektir.

## Modül 1 · Copilot nasıl çalışır?

### D1 · Editörde öneriler ve D2 · Ask, Plan, Agent

**Amaç:** Copilot'un editördeki üç öneri biçimini (tamamlama, Next Edit Suggestions, inline chat) ve sohbetteki üç modu (Ask, Plan, Agent) aynı iş üzerinde karşılaştırmak.

**Başlangıç noktası:** `main`

#### Hazırlık · Çalışma branch'i

   > 💡 **Neden?** Demolarda yaptığımız değişiklikleri main'den ayrı bir branch'te tutuyoruz. Böylece main her zaman temiz bir başlangıç noktası kalıyor; bir şey ters giderse tek komutla geri dönebiliyoruz.

1. **Terminal**:

   ```powershell
   git switch -c canli/d1-m1 main
   ```

   > 🔎 **Sonucu nasıl okuruz?** "Switched to a new branch 'canli/d1-m1'": main'den yeni bir branch açıldı ve ona geçtik. Sol alttaki branch adı da değişti.

#### D1 · Yorumdan kod: tamamlama

   > 💡 **Neden?** En basit kullanım: ne istediğimizi bir yorum satırıyla anlatıyoruz, Copilot devamını tahmin ediyor. Yorum aslında küçük bir prompt.

1. **Git:** Explorer → src → Bank.Api → Services → TransferLimitChecker.cs.
2. **Tıkla:** IsWithinDailyLimit metodunun kapanış süslü parantezinin (}) sonuna tıkla, Enter ile yeni boş satır aç. Sınıfın son } işaretinin üstünde olduğundan emin ol.
3. **Editör** — yaz, sonra Enter:

   ```csharp
   // IBAN'ın TR ile başlayıp 26 karakter olduğunu kontrol eden metot
   ```

4. **Beklenen:** Bir iki saniye içinde yeni satırda gri renkte bir metot önerisi. Tab ile kabul et.

   > ⚠️ **Sorun olursa:** Öneri gelmezse: (1) Alt+\ ile öneriyi elle çağır. (2) Yeni satıra "public static bool " yaz ve bekle. (3) Sağ alttaki Copilot simgesine tıkla: kod tamamlama açık mı, "Snooze" etkin mi, oturum açık mı? (4) Hâlâ yoksa Ctrl+I ile inline chat'e aynı yorumu yaz.

#### D1 · Next Edit Suggestions

   > 💡 **Neden?** Next Edit Suggestions (NES), yaptığımız bir değişikliğin devamını tahmin ediyor: bir parametrenin adını değiştirince, o adın geçtiği diğer satırları da önerir.

1. **Tıkla:** Metodun tanım satırına (imzasına) git: "public bool IsWithinDailyLimit(decimal amount, ...)". Parantez içindeki amount kelimesine çift tıkla ve transferAmount yaz. (F2 ile yeniden adlandırma YAPMA; F2 her yeri kendisi değiştirir ve Copilot'u göstermez.)
2. **Beklenen:** Metodun gövdesindeki amount satırının yanında, satır numarasının solunda bir ok işareti ve gri bir düzenleme önerisi.
3. **Tıkla:** Tab ile o satıra atla, tekrar Tab ile kabul et.

#### D1 · Inline chat

   > 💡 **Neden?** Inline chat, sohbet paneline gitmeden, tam bulunduğumuz yerde küçük ve hedefli işler için. Seçtiğimiz kod otomatik olarak bağlama giriyor.

1. **Tıkla:** IsWithinDailyLimit metodunun tamamını fareyle seç → Ctrl+I.
2. **Inline chat**:

   ```text
   Bu metoda XML dokümantasyon yorumu ekle; parametreleri ve dönüş değerini Türkçe açıkla.
   ```

3. **Beklenen:** Editörde yeşil satırlarla önerilen değişiklik → Accept (ya da Ctrl+Enter).

   > 💡 **Neden?** D1'de yaptığımız üç değişikliği geri alıyoruz ki D2 temiz koddan başlasın.

4. **Terminal** — D1 değişikliklerini geri al:

   ```powershell
   git restore .
   ```

   > 🔎 **Sonucu nasıl okuruz?** Komut sessizce biter; çıktı vermemesi başarılı olduğu anlamına gelir. Editördeki dosya eski haline döner.

#### D2 · Ask

   > 💡 **Neden?** Aynı işi üç moda vereceğiz ve kim ne yaptı, hangi dosya değişti, buna bakacağız. Ask yalnızca anlatır.

1. **Tıkla:** Chat panelinde + ile yeni sohbet → alttaki mod seçiciden Ask.
2. **Chat · Ask**:

   ```text
   Hesaptan para çekme (withdrawal) endpoint'i ekle: POST /accounts/{id}/withdrawals.
   Bakiye yetersizse hata dönsün.
   ```

3. **Beklenen:** Açıklama ve kod blokları. Explorer'da ve soldaki Source Control simgesinde hiçbir değişiklik yok.

#### D2 · Plan

   > 💡 **Neden?** Plan modu önce kodu araştırır, belirsiz noktaları sorar ve bir yol haritası çıkarır; yine dosyaya dokunmaz. Yanlış anlaşılmayı kod yazılmadan yakalamanın yolu bu.

1. **Tıkla:** Chat panelinde + ile yeni sohbet → mod seçiciden Plan → aynı prompt'u yapıştır, Enter.
2. **Chat · Plan**:

   ```text
   Hesaptan para çekme (withdrawal) endpoint'i ekle: POST /accounts/{id}/withdrawals.
   Bakiye yetersizse hata dönsün.
   ```

   > ⚠️ **Sorun olursa:** Plan listede yoksa oturum hedefi Local değildir (en alttaki "Copilot" yazısına tıkla → Local). Hızlı yol: mesajın başına /plan yaz.

3. **Beklenen:** Plan çoğu zaman varsayımlarını sıralayıp şıklı bir soru sorar (ör. "yalnız bakiye güncellemesi mi, ayrı çekim kaydı da mı?"). Kapsamı küçük tutan şıkkı seç ya da en alttaki kutuya kendi cevabını yaz:
4. **Chat · Plan** — soru gelmezse bu adımı atla:

   ```text
   Varsayımlar uygun: yalnız bakiye güncellemesi, ayrı çekim kaydı yok. Hata kodu withdrawal.insufficient_balance olsun.
   ```

#### D2 · Agent

   > 💡 **Neden?** Agent modu planı uygular: dosyaları değiştirir, derler, hata varsa düzeltir. Burada dikkat edeceğimiz şey, her komut için bizden izin istemesi.

1. **Tıkla:** Planın altındaki "Start Implementation" düğmesine tıkla. Düğme yoksa mod seçiciden Agent'ı seç ve aynı sohbette yaz:
2. **Chat · Agent**:

   ```text
   Planı uygula.
   ```

3. **Beklenen:** Terminal komutu için onay kutusu: "Run dotnet build?" → Allow.

   > ✅ **Onay (Allow) istenirse:** Ajana yalnızca bu tek komutu, dotnet build'i, bu terminalde çalıştırma izni veriyoruz. Derleme kodu değiştirmez; ajan çıktıyı okuyup hata var mı diye bakacak. "Always allow" ya da oturum boyu izin seçeneklerini kullanmıyoruz; her komutu görmek istiyoruz.

4. **Tıkla:** Sohbetteki "N files changed" özetinden bir dosyaya tıkla → diff görünümünü göster.

   > 🔎 **Sonucu nasıl okuruz?** Diff ekranında solda eski, sağda yeni hal var. Ajanın değiştirdiği her satır burada; okumadan kabul etmiyoruz.

   > 💡 **Neden?** Agent değişiklikleri doğrudan dosyalara yazıyor. Geri almak için git kullanıyoruz: restore değişen dosyaları eski haline getirir, clean yeni eklenen dosyaları siler.

5. **Terminal** — temizlik:

   ```powershell
   git restore .
   git clean -fd src tests
   ```

   > 🔎 **Sonucu nasıl okuruz?** git clean silinen dosyaları "Removing src/..." satırlarıyla listeler; bunlar ajanın eklediği yeni dosyalar.

   > ⚠️ **Sorun olursa:** Agent 3 dakikayı aşarsa sohbetteki kare (Stop) düğmesiyle durdur, o ana kadar değişen dosyaları göster ve temizlik komutunu çalıştır. Amaç davranış farkını göstermek; özelliğin bitmesi gerekmiyor. Görsel yol: sol kenarda Source Control (Ctrl+Shift+G) → Changes başlığının yanındaki kıvrık ok (Discard All Changes).

**Bu demodan çıkan ders:** Ask öğrenmek için, Plan düşünmek için, Agent yapmak içindir. Değişiklik yapan tek mod Agent; onun yaptığı her şeyi okuyup kabul etmek bizim işimiz.

### D3 · Aynı soru, farklı cevap ve D4 · Olmayan paket

**Amaç:** Modelin olasılıksal çalıştığını (aynı soruya farklı cevap) ve kendinden emin şekilde uydurabildiğini (olmayan paket) kendi gözümüzle görmek.

**Başlangıç noktası:** `canli/d1-m1`

#### D3 · Aynı soru, üç yeni sohbet

   > 💡 **Neden?** Her yeni sohbet sıfırdan başlar. Aynı soruyu üç ayrı sohbette sorup cevapları karşılaştırınca modelin olasılıksal çalıştığını görüyoruz.

1. **Hazırlık:** Windows Not Defteri'ni aç (Başlat → "Not Defteri"), ekranın sağ yarısına yerleştir.
2. **Tıkla:** Chat panelinde + ile yeni sohbet → Ask.
3. **Chat · Ask**:

   ```text
   C# ile Türk IBAN numarasını doğrulayan bir metot yaz. Sadece kodu ver.
   ```

4. **Tıkla:** Cevaptaki kod bloğunun sağ üstündeki Copy simgesi → Not Defteri'ne yapıştır, üstüne "1" yaz. İki kez daha: + yeni sohbet → Ask → aynı prompt → "2" ve "3" başlığıyla yapıştır.

#### D4 · Olmayan paket

   > 💡 **Neden?** Var olmayan bir paketi isteyip modelin "yok" mu dediğine, yoksa uydurduğuna mı bakıyoruz. Uydurulan paket adları gerçek bir saldırı yöntemine dönüştü.

1. **Tıkla:** + yeni sohbet → Ask.
2. **Chat · Ask**:

   ```text
   Projeye Microsoft.Banking.Iban NuGet paketini ekleyip IBAN doğrulamasını onunla yap. Hangi sınıf ve metodu kullanmalıyım?
   ```

3. **Beklenen:** İki olasılık var, ikisi de işine yarar: (a) böyle bir paket olmadığını söyler; (b) paket adı, sınıf ve metot uydurur.
4. **Git:** Tarayıcıda yeni sekme (Ctrl+T) → adres çubuğuna yaz:
5. **Tarayıcı adres çubuğu**:

   ```text
   https://www.nuget.org/packages?q=Microsoft.Banking.Iban
   ```

   > 🔎 **Sonucu nasıl okuruz?** Sonuç listesinde bu adla bir paket yok.

   > 💡 **Neden?** Modül sonu: çalışma branch'indeki her şeyi temizleyip main'e dönüyoruz.

6. **Terminal** — modül sonu temizlik:

   ```powershell
   git restore .
   git clean -fd src tests
   git switch main
   ```

7. **Hazırlık:** Not Defteri'ni kaydetmeden kapat.

**Bu demodan çıkan ders:** Copilot'un önerdiği her paketi nuget.org'da, her kodu testle doğrulayın.

## Modül 2 · Prompt ve bağlam mühendisliği

### D5 · Kötü prompt, iyi prompt

**Amaç:** Tek cümlelik bir prompt ile hedefi, kısıtı ve kabul kriteri olan bir prompt arasındaki farkı aynı kod üzerinde görmek.

**Başlangıç noktası:** `main`

#### Hazırlık · Çalışma branch'i

1. **Terminal** — Modül 2 çalışma branch'i:

   ```powershell
   git switch -c canli/d1-m2 main
   ```

   > 💡 **Neden?** Modül 2'nin bütün denemelerini bu branch'te yapacağız; main'e dokunmuyoruz.

#### Kötü prompt

   > 💡 **Neden?** Tek cümlelik bir prompt verip ajanın kaç şeyi tahmin etmek zorunda kaldığını görüyoruz.

1. **Tıkla:** Chat → + yeni sohbet → Agent.
2. **Chat · Agent**:

   ```text
   Transfere validasyon ekle.
   ```

3. **Beklenen:** Ajan bir şeyler ekler; ne ekleyeceği belli değil. Komut onayı gelirse Allow.

   > ✅ **Onay (Allow) istenirse:** dotnet build ya da dotnet test için izin veriyoruz: kodu derleyip testleri çalıştıracak, dosya değiştirmeyecek.

4. **Tıkla:** "N files changed" özetinden değişen dosyaları göster.
5. **Terminal** — kötü prompt'un değişikliklerini geri al:

   ```powershell
   git restore .
   git clean -fd src tests
   ```

#### İyi prompt

   > 💡 **Neden?** Aynı işi hedef, kısıt ve kabul kriteriyle veriyoruz. Son satırdaki "test yaz ve dotnet test ile doğrula" ajana işin ne zaman bittiğini söylüyor.

1. **Tıkla:** + yeni sohbet → Agent. Prompt'u PROMPTLAR-GUN1.md'den kopyala.
2. **Chat · Agent**:

   ```text
   TransferService.TransferAsync metoduna açıklama (Description) alanı için doğrulama ekle:
   - Açıklama boş olabilir, en fazla 140 karakter olabilir.
   - Aşarsa ErrorType.Validation türünde "transfer.description_too_long" hatası dönsün.
   Mevcut Result<T> ve TransferErrors desenini kullan, exception fırlatma.
   Kural için tests/Bank.Api.Tests altında xUnit testi yaz ve dotnet test ile doğrula.
   ```

3. **Beklenen:** TransferErrors.cs'e yeni hata, TransferService.cs'e kontrol, test projesine yeni test(ler). dotnet test için onay → Allow.

   > ✅ **Onay (Allow) istenirse:** dotnet test, bütün testleri derleyip çalıştırır. Ajan sonucu okuyup kırmızı test varsa düzeltecek. Dosya değiştirme izni değil, yalnızca bu komutu çalıştırma izni.

   > 🔎 **Sonucu nasıl okuruz?** Test çıktısının sonunda "Passed!  - Failed: 0, Passed: N" satırı: kaç testin çalıştığı ve hepsinin geçtiği. Failed sıfırdan büyükse ajan döngüye geri döner.

#### Temizlik

1. **Terminal** — D5 değişikliklerini geri al:

   ```powershell
   git restore src tests
   git clean -fd src tests
   ```

   > ⚠️ **Sorun olursa:** Kötü prompt'taki ajan uzun sürerse 1,5 dakikada Stop; amaç farkı göstermek. İyi prompt'ta dotnet test başarısız olursa ajanın düzeltmesini bekle; 2 dakikayı geçerse Stop → temizlik komutu ve sonucu sözlü anlat.

**Bu demodan çıkan ders:** Ajanın işi ne zaman bitireceğini kabul kriteri belirler; "test yaz ve dotnet test ile doğrula" demek ajanı kendi işini kontrol etmeye zorlar.

### D6 · Katmanlı özelleştirme

**Amaç:** Copilot'a kurumun kurallarını öğreten dosyaları (.github altındaki talimat, prompt, skill ve ajan dosyaları) tek tek ekleyip her birinin sonuca etkisini görmek.

**Başlangıç noktası:** `canli/d1-m2 (D5'te açıldı)` · **Bitmiş hali:** `yedek/d1-m2-son`

#### Hazırlık · Geri alma komutu

   > 💡 **Neden?** Her adımda yalnızca kodu ve testleri geri alacağız; .github klasörüne dokunmayacağız. Böylece eklediğimiz katmanlar birikerek ilerliyor.

1. **Terminal** — her adımın sonundaki geri alma komutu (şimdi çalıştırma):

   ```powershell
   git restore src tests
   git clean -fd src tests
   ```

#### Adım 1 · Hiçbir özelleştirme yokken

   > 💡 **Neden?** Önce Copilot'un bankamızı hiç tanımadığı hali görüyoruz; sonraki adımlarda farkı buna göre ölçeceğiz.

1. **Git:** Explorer'da depo köküne bak: .github klasörü yok.
2. **Tıkla:** + yeni sohbet → Agent.
3. **Chat · Agent**:

   ```text
   Hesaba para yatırma endpoint'i ekle: POST /accounts/{id}/deposits
   ```

4. **Beklenen:** Ajan bir uç nokta ekler. Komut onayları için Allow.

   > ✅ **Onay (Allow) istenirse:** dotnet build ve dotnet test komutlarına izin veriyoruz; ajan kendi değişikliğinin derlenip derlenmediğine bakacak.

5. **Tıkla:** Değişen dosyaları aç. Kâğıda altı soruyu yaz ve cevapla: exception mı fırlattı? Result<T> kullandı mı? Tutar decimal mı? Test yazdı mı? 201 Created dönüyor mu? .http dosyasına örnek ekledi mi?
6. **Terminal** — Adım 1'i geri al:

   ```powershell
   git restore src tests
   git clean -fd src tests
   ```

#### Adım 2 · Depo talimatları: copilot-instructions.md

   > 💡 **Neden?** Bankanın kodlama kurallarını tek bir dosyaya yazıyoruz. Copilot bu dosyayı her prompt'a kendiliğinden ekler; kuralları her seferinde tekrar yazmak zorunda kalmayız.

1. **Terminal** — hazır dosyayı yedek branch'ten getir:

   ```powershell
   git checkout yedek/d1-m2-son -- .github/copilot-instructions.md
   ```

   > 🔎 **Sonucu nasıl okuruz?** Komut çıktı vermez. Bu komut, yedek/d1-m2-son branch'indeki dosyanın bir kopyasını bulunduğumuz branch'e getirir; branch değiştirmez. Explorer'da .github klasörü belirdi.

2. **Git:** Explorer → .github → copilot-instructions.md dosyasını aç.
3. **Tıkla:** + yeni sohbet → Agent → aynı prompt.
4. **Chat · Agent**:

   ```text
   Hesaba para yatırma endpoint'i ekle: POST /accounts/{id}/deposits
   ```

5. **Beklenen:** Result<T>, decimal, test ve sonunda build + test. Cevabın başındaki "Used N references" satırına tıkla: listede copilot-instructions.md görünür. Onaylarda Allow.
6. **Terminal** — Adım 2'yi geri al:

   ```powershell
   git restore src tests
   git clean -fd src tests
   ```

#### Adım 3 · Kapsamlı talimatlar: applyTo

   > 💡 **Neden?** Bazı kurallar yalnızca belirli dosyalar için geçerli: test kuralları test dosyaları için. Bunları ayrı dosyaya yazıp applyTo ile kapsamını veriyoruz; ilgisiz işlerde bağlamı kirletmiyorlar.

1. **Terminal**:

   ```powershell
   git checkout yedek/d1-m2-son -- .github/instructions
   ```

2. **Git:** .github → instructions → tests.instructions.md dosyasını aç; en üstteki applyTo: "tests/**/*.cs" satırını göster.

#### Adım 4 · Prompt dosyası: /yeni-endpoint

   > 💡 **Neden?** Ekipte sık yapılan bir işin tarifini bir kez yazıp herkesin aynı şekilde çağırmasını sağlıyoruz. Bir prompt dosyası, kurumun "yeni endpoint nasıl eklenir" bilgisini taşır.

1. **Terminal**:

   ```powershell
   git checkout yedek/d1-m2-son -- .github/prompts
   ```

2. **Git:** .github → prompts → yeni-endpoint.prompt.md dosyasını aç. Göster: üstteki description ve agent satırları, gövdedeki ${input:endpoint} alanı ve altı adım.
3. **Tıkla:** + yeni sohbet. Mesaj kutusuna sadece / yaz: açılan listede yeni-endpoint görünür.

   > ⚠️ **Sorun olursa:** yeni-endpoint listede yoksa oturum hedefi Local değildir: en alttaki "Copilot" yazısına tıkla → Local → + yeni sohbet → tekrar / yaz. Hâlâ yoksa Ctrl+Shift+P → "Chat: Configure Prompt Files" ile dosyanın görünüp görünmediğine bak.

4. **Chat · Agent**:

   ```text
   /yeni-endpoint Hesaba para yatırma
   ```

5. **Beklenen:** Kısa bir plan, sonra Contracts altında record'lar, servis, endpoint, testler ve Bank.Api.http'ye örnek istek. Onaylarda Allow.

   > ✅ **Onay (Allow) istenirse:** Ajan dotnet build ve dotnet test çalıştırmak istiyor; tarifin son adımı "derle ve test et". İzin yalnızca bu komutlar için.

6. **Önemli:** Bu adımın değişikliklerini GERİ ALMA. Adım 6'da güvenlik ajanı bunları inceleyecek.

#### Adım 5 · Skill: konu gelince açılan paket

   > 💡 **Neden?** Skill'i ilk kez görenler için: skill, bir konuda nasıl çalışılacağını anlatan talimatlar ve şablonlardan oluşan bir klasör. Copilot başta yalnızca açıklama satırını okur; prompt o konuya değince klasörün tamamını yükler. Böylece uzman bilgisi her prompt'ta bağlamı doldurmaz, yalnızca gerektiğinde gelir.

1. **Terminal**:

   ```powershell
   git checkout yedek/d1-m2-son -- .github/skills
   ```

2. **Git:** .github → skills → banka-api-standartlari klasörünü aç: SKILL.md, endpoint-sablonu.md, hata-yaniti-formati.md. SKILL.md'de description satırını göster.

#### Adım 6 · Özel ajan: guvenlik-inceleyici

   > 💡 **Neden?** Bir rolü ve o rolün kullanabileceği araçları tanımlayan bir ajan ekliyoruz. Güvenlik inceleyici yalnızca okuyabilir; dosya düzenleme ve terminal aracı listesinde yok. Yani istese de kod değiştiremez.

1. **Terminal**:

   ```powershell
   git checkout yedek/d1-m2-son -- .github/agents
   ```

2. **Git:** .github → agents → guvenlik-inceleyici.agent.md dosyasını aç; tools satırını incele: read/..., search/... araçları var, düzenleme ve terminal yok.
3. **Tıkla:** + yeni sohbet → ajan seçiciyi aç: Ask, Plan, Agent'ın altında guvenlik-inceleyici görünür → seç.
4. **Chat · guvenlik-inceleyici**:

   ```text
   #file:src/Bank.Api/Services/AccountService.cs #file:src/Bank.Api/Endpoints/AccountEndpoints.cs dosyalarındaki para yatırma kodunu incele.
   ```

5. **Beklenen:** Önem sırasına göre bulgu tablosu. Hiçbir dosya değişmez.

   > ⚠️ **Sorun olursa:** Ajan dosyayı okuyamıyorum derse oturum hedefinin Local olduğunu kontrol et. Ajan dosyasında "unknown tool" uyarısı görürsen o araç senin VS Code sürümünde başka adla geçiyor: tools satırının üstündeki Configure Tools bağlantısıyla araçları listeden seçtir; VS Code satırı doğru adlarla yeniden yazar.

#### Kapanış · Bitmiş hali göster

   > 💡 **Neden?** Modülün bitmiş haline geçiyoruz; takip edenler de buradan devam edebilir.

1. **Terminal** — temizle, bitmiş hale geç, testleri çalıştır:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch yedek/d1-m2-son
   dotnet test ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** restore ve clean çalışma alanını tamamen temizler; switch bitmiş branch'e geçer; dotnet test sonunda "Passed: 16" yazar.

2. **Git:** Explorer'da .github ağacını aç ve incele.

   > ⚠️ **Sorun olursa:** Bir adım 6 dakikayı aşarsa Stop → geri alma komutu → sonraki adım. Zaman çok sıkışırsa Adım 3'ü yalnızca dosyayı göstererek 30 saniyede geç. "index.lock: File exists" hatası: başka bir git işlemi yarıda kalmış; Remove-Item .git\index.lock komutuyla kilidi sil, komutu tekrarla.

**Bu demodan çıkan ders:** Standartlar bir dokümanda değil, depoda yaşamalı. Kuralı bir kez dosyaya yazınca her geliştiricinin Copilot'u aynı kurala uyar.

## Modül 3 · Spec-odaklı geliştirme

### D7 · Spec Kit ile virman özelliği

**Amaç:** İş biriminin tek cümlelik talebini Spec Kit ile önce ilkelere (anayasa), sonra spesifikasyona, netleştirilmiş kararlara, teknik plana ve görev listesine dönüştürmek; bitmiş özelliği test ve HTTP isteğiyle doğrulamak.

**Başlangıç noktası:** `yedek/d1-m2-son` · **Bitmiş hali:** `yedek/d1-m3-son`

#### Hazırlık · Temiz başlangıç

   > 💡 **Neden?** Önceki modülden kalan her şeyi temizleyip Modül 2'nin bitmiş halinden yeni bir çalışma branch'i açıyoruz. Spec Kit, talimat dosyalarımızın üzerine kurulacak.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch -c canli/d1-m3 yedek/d1-m2-son
   ```

#### Adım 1 · Spec Kit kurulumu

   > 💡 **Neden?** Spec Kit'i bu depoya kuruyoruz. Komut; ajanın kullanacağı şablonları .specify/templates altına, yardımcı PowerShell betiklerini .specify/scripts altına, sohbetten çağıracağımız /speckit-... komutlarını da .github/skills altına skill olarak yazar. Bir kez yapılır ve dosyalar depoyla birlikte sürümlenir; ekipte herkes aynı akışı kullanır.

1. **Terminal**:

   ```powershell
   specify init --here --integration copilot --script ps --force --non-interactive
   ```

   > 🔎 **Sonucu nasıl okuruz?** --here: bulunduğumuz klasöre kur. --integration copilot: GitHub Copilot için skill'leri oluştur. --script ps: yardımcı betikler PowerShell olsun. --force: klasör boş değil, yine de kur. --non-interactive: soru sorma. Çıktının sonunda kurulan dosyalar ve "Next Steps" kutusu görünür. Explorer'da yeni .specify klasörü ve .github/skills altında speckit-... klasörleri belirir.

   > ⚠️ **Sorun olursa:** "specify is not recognized": Spec Kit CLI kurulu değil. Hızlı çözüm, kurmadan çalıştırmak: uvx --from specify-cli==1.2.4 specify init --here --integration copilot --script ps --force --non-interactive. uv da yoksa: winget install --id=astral-sh.uv -e → VS Code'u kapatıp aç → uv tool install specify-cli==1.2.4. Takılırsanız: yedekteki kurulumu getir: git checkout yedek/d1-m3-son -- .specify .github/skills ve akışı o dosyalarla sürdür. "running scripts is disabled" hatası: Set-ExecutionPolicy -Scope CurrentUser RemoteSigned.

#### Adım 2 · Anayasa

   > 💡 **Neden?** Anayasa, projenin değişmez ilkelerini yazdığımız dosya. Sonraki her adım (spec, plan, görevler) bu ilkelere göre kontrol edilecek. İlkeleri biz veriyoruz; ajan onları gerekçeli, ölçülebilir maddelere çeviriyor.

1. **Tıkla:** + yeni sohbet → Agent. Mesaj kutusuna /speckit- yaz: komut listesi açılır.
2. **Chat · Agent**:

   ```text
   /speckit-constitution Erdağı Bank Hesap API'si için anayasa: (1) Önce test, değiştirilemez. (2) Müşteri adı, TCKN ve IBAN log'a ve hata mesajlarına yazılmaz. (3) Para her zaman decimal. (4) İş hataları exception değil Result<T> ile döner, HTTP'de ProblemDetails. (5) Sadelik: yeni paket veya katman ancak gerekçesiyle. Ayrıca: müşteri yalnızca kendi hesabında işlem yapar; SQL yalnızca parametreli; YZ kodu insan incelemesi olmadan main'e girmez.
   ```

3. **Beklenen:** Ajan bir PowerShell betiği çalıştırmak için onay ister (resolve-template.ps1) → Allow.

   > ✅ **Onay (Allow) istenirse:** Ajan Spec Kit'in kendi betiğini çalıştırıp anayasa şablonunun yerini buluyor. Betik yalnızca dosya yolu okuyup bildiriyor, bir şey değiştirmiyor; depoda .specify/scripts/powershell altında, açıp okuyabiliriz.

4. **Git:** .specify → memory → constitution.md dosyasını aç; beş ilkeyi ve sürüm satırını göster.

   > ⚠️ **Sorun olursa:** Ajan "dosyalara erişemiyorum / komut çalıştıramıyorum" derse sohbet yanlış modda: oturum hedefi Local mi, mod Agent mı (Ask ve Plan dosyaya yazamaz), araç simgesinde (Configure Tools) terminal ve edit araçları işaretli mi, klasör Restricted Mode'da mı? Düzelt, yeni sohbet aç, tekrar gönder. Takılırsanız: git checkout yedek/d1-m3-son -- .specify/memory/constitution.md

#### Adım 3 · Spesifikasyon

   > 💡 **Neden?** Talebi iş biriminin diliyle spec'e dönüştürüyoruz: kim, ne istiyor, neden; kabul senaryoları; gereksinimler. Teknoloji yok; spec'i iş analisti okuyup onaylayabilmeli.

1. **Tıkla:** + yeni sohbet → Agent.
2. **Chat · Agent**:

   ```text
   /speckit-specify Müşteri kendi hesapları arasında para aktarabilsin (virman). Bu aktarım, başka kişilere yapılan transferler için tanımlı günlük limitten bağımsız olsun.
   ```

3. **Beklenen:** Betik onayı (create-new-feature.ps1) → Allow.

   > ✅ **Onay (Allow) istenirse:** Bu betik specs klasörü altında özelliğe numaralı bir klasör (001-...) açar ve spec şablonunu oraya kopyalar. Yalnızca o klasörü oluşturur.

4. **Git:** specs → 001-... → spec.md dosyasını aç. Sırayla göster: kullanıcı hikâyeleri; Given / When / Then senaryoları; FR-001, FR-002 ... gereksinimleri; Ctrl+F ile "NEEDS" ara.

#### Adım 4 · Netleştirme

   > 💡 **Neden?** Ajan spec'teki belirsizlikleri en fazla beş soruyla bize soruyor. Bu soruların cevabı iş kararı; kod yazılmadan önce verilmeli. Burada iş analistinin rolünü üstleniyoruz.

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-clarify
   ```

2. **Beklenen:** Ajan soruları tek tek sorar; çoğunda şıklar ve bir öneri vardır. Bir şık harfi yazarak ya da kendi cümlenle cevap ver. Betik onayı gelirse Allow (yalnızca spec dosyasının yolunu bulur).
3. **Cevapların:** Sorunun konusuna göre şu kararları ver:
4. **Chat · Agent** — limit sorusu:

   ```text
   Virman günlük limite tabi değil ve sonraki dış transferlerin limit hesabına da katılmaz.
   ```

5. **Chat · Agent** — başkasının hesabı sorusu:

   ```text
   Hesaplardan biri müşteriye ait değilse "Hesap bulunamadı" (404) dönsün; başkasının hesabının varlığı ele verilmesin.
   ```

6. **Chat · Agent** — para birimi sorusu:

   ```text
   Farklı para birimli hesaplar arasında virman bu sürümde yok.
   ```

7. **Chat · Agent** — ücret sorusu:

   ```text
   Virman için ücret alınmaz.
   ```

8. **Diğer:** Listede olmayan bir soru gelirse ajanın önerdiği şıkkı kabul et.
9. **Git:** spec.md'de en alta in: "## Clarifications" bölümü ve cevapların gereksinimlere işlendiğini göster.

#### Adım 5 · Plan

   > 💡 **Neden?** Spec "ne"yi anlattı; plan "nasıl"ı anlatır. Teknik kararları biz veriyoruz, ajan bunları plana döküyor ve anayasaya göre tek tek denetliyor.

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-plan Mevcut .NET 10 Minimal API, EF Core SQLite ve servis + depo + endpoint desenini kullan. Yeni paket ekleme. Virmanları mevcut Transfer tablosunda yeni bir Kind (External/Internal) alanıyla tut; günlük limit toplamı yalnızca External transferleri saysın. Uç: POST /customers/{customerId}/internal-transfers.
   ```

2. **Beklenen:** Betik onayı (setup-plan.ps1) → Allow.

   > ✅ **Onay (Allow) istenirse:** Betik plan şablonunu özellik klasörüne kopyalar. Ajan sonra mevcut kodu okuyup plan.md, research.md, data-model.md ve contracts klasörünü yazar.

3. **Git:** plan.md'yi aç → Ctrl+F "Constitution" → Constitution Check bölümü.

#### Adım 6 · Görevler

   > 💡 **Neden?** Plan, sıralı ve numaralı görevlere bölünüyor. Bu liste hem ajanın uygulama sırası hem de iş takibi için kullanılabilir.

1. **Chat · Agent** — aynı sohbette:

   ```text
   /speckit-tasks
   ```

   > ✅ **Onay (Allow) istenirse:** check-prerequisites.ps1 betiği spec ve plan dosyalarının yerinde olduğunu kontrol eder; bir şey değiştirmez.

2. **Git:** tasks.md'yi aç. Göster: T001, T002 ... numaraları, [P] etiketi (paralel yapılabilir), [US1] etiketi (hangi kullanıcı hikâyesine ait).

#### Adım 7 · Uygulama: bitmiş hale geçiş ve doğrulama

   > 💡 **Neden?** Son adım /speckit-implement: ajan tasks.md'yi sırayla uygular. Bu adım 10–15 dakika sürdüğü için canlıda çalıştırmıyoruz; aynı akışın daha önce tamamlanmış haline geçip sonucu doğruluyoruz.

1. **Terminal**:

   ```powershell
   git restore --staged --worktree .
   git clean -fd
   git switch yedek/d1-m3-son
   ```

2. **Git:** specs → 001-kendi-hesaplar-arasi-virman: spec.md, plan.md, tasks.md, contracts. Sonra src/Bank.Api/Services/InternalTransferService.cs: koddaki // FR-002, // FR-007 yorumlarını göster.
3. **Terminal**:

   ```powershell
   dotnet test ErdagiBank.slnx
   ```

   > 🔎 **Sonucu nasıl okuruz?** Sonda "Passed: 26": virman için eklenen testlerle birlikte hepsi geçti.

4. **Terminal**:

   ```powershell
   dotnet run --project src/Bank.Api
   ```

5. **Tıkla:** Bank.Api.http → "Kendi hesaplar arası virman: 15.000 TL" isteğinin Send Request'i → 201 Created (Ayşe'nin günlük limiti 10.000 TL olduğu halde). Sonra "Virman: başkasına ait hesap" → 404 ve ProblemDetails gövdesi.

   > 🔎 **Sonucu nasıl okuruz?** 201: virman limite takılmadı. 404: başkasının hesabı için "bulunamadı" döndü. ProblemDetails gövdesinde "type", "title" ve hata kodu var; anayasadaki dördüncü ilke.

6. **Tıkla:** API terminalinde Ctrl+C.

   > ⚠️ **Sorun olursa:** Adım 2–6'dan biri 6 dakikayı aşarsa: Stop → doğrudan Adım 7'ye geç ve kalan adımları yedekteki specs klasörü üzerinden göster (dosyalar aynı yapıda).

**Bu demodan çıkan ders:** Belirsizlik kod yazılmadan, spec'te çözülür. Clarify adımında verdiğimiz iş kararları koda ve testlere kadar izlenebilir.

