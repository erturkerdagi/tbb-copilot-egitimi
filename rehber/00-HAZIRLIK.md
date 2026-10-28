# Eğitimden önce: kurulum ve kontrol listesi

Bu dosyadaki adımları **eğitimden en az bir gün önce** yapın. Toplam süre yaklaşık 30–45 dakika. Kurulumların çoğu bir kez yapılır.

Eğitimi yalnızca izleyecekseniz hiçbir kurulum gerekmez. Demoları kendi makinenizde takip etmek ya da eğitimden sonra tekrar etmek istiyorsanız aşağıdakilerin hepsi gerekli.

Komutlar Windows ve PowerShell için yazıldı. macOS ve Linux notları en sonda.

## Özet: neye ihtiyacınız var?

| # | Ne | Neden | Zorunlu mu? |
| --- | --- | --- | --- |
| 1 | GitHub hesabı ve GitHub Copilot erişimi | Copilot'u kullanmak için | Evet |
| 2 | Git | Depoyu indirmek, branch'ler arasında geçmek, değişiklikleri geri almak | Evet |
| 3 | .NET 10 SDK | Hesap API'sini derlemek, çalıştırmak, test etmek | Evet |
| 4 | Visual Studio Code ve eklentiler | Bütün demolar VS Code'da | Evet |
| 5 | uv ve Spec Kit (`specify`) | Gün 1 · Modül 3 ve Gün 3 atölyesi | Evet |
| 6 | .NET Framework 4.8 Developer Pack | Gün 2 · Modül 5'te 2014 sistemini eski haliyle derlemek | Yalnızca Windows; isteğe bağlı |
| 7 | Docker Desktop | Gün 3 · Modül 7'de image'ı yerelde derlemek | İsteğe bağlı |
| 8 | Deponun kendi kopyanız (fork) | Gün 3'te pull request, CI ve issue adımları | Gün 3'ü takip edecekseniz |

## 1. GitHub hesabı ve Copilot erişimi

1. [github.com](https://github.com) adresinde bir hesabınız olsun.
2. Copilot erişiminizi kontrol edin: [github.com/settings/copilot](https://github.com/settings/copilot) sayfasında hangi plandasınız?
   - Kurumunuz **Copilot Business** ya da **Enterprise** lisansı verdiyse onu kullanın.
   - Bireysel olarak **Copilot Pro** ya da **Pro+** da olur.
   - **Copilot Free** ile demoların çoğu denenebilir, ama aylık kullanım hakkı sınırlı; üç günü takip etmeye yetmeyebilir.
3. Kurumsal lisansta bazı özellikler (Agent modu, cloud agent, code review, MCP) yöneticiniz tarafından kapatılmış olabilir. Emin değilseniz yöneticinize sorun.

> **Kurum ağı:** Kurumsal bir ağdaysanız proxy ya da güvenlik duvarının GitHub, GitHub Copilot, NuGet (nuget.org) ve PyPI'ye (pypi.org) erişime izin verdiğinden emin olun. Copilot sohbetinde "bağlantı hatası" görürseniz sebebi çoğu zaman budur.

## 2. Git

```powershell
winget install --id Git.Git -e
```

Kurulumdan sonra **yeni bir terminal açın** ve kontrol edin:

```powershell
git --version
```

Sonuç `git version 2.x` olmalı. İlk kez kullanıyorsanız adınızı ve e-postanızı tanıtın:

```powershell
git config --global user.name "Adınız Soyadınız"
git config --global user.email "eposta@ornek.com"
```

## 3. .NET 10 SDK

```powershell
winget install Microsoft.DotNet.SDK.10
```

Ya da [dotnet.microsoft.com/download/dotnet/10.0](https://dotnet.microsoft.com/download/dotnet/10.0) adresinden **SDK**'yı (Runtime değil) indirin. Kontrol:

```powershell
dotnet --list-sdks
```

Listede `10.0.` ile başlayan bir satır olmalı. En güncel 10.0 sürümünü kurun; güvenlik düzeltmeleri aylık yayımlanıyor.

## 4. Visual Studio Code ve eklentiler

```powershell
winget install Microsoft.VisualStudioCode
```

VS Code zaten kuruluysa **en güncel sürüme** güncelleyin: Help → Check for Updates. Copilot'un sohbet ekranı sık değişiyor; eğitimdeki ekranlar güncel sürüme göre.

Eklentileri tek seferde kurmak için VS Code'u kapatıp PowerShell'de çalıştırın:

```powershell
code --install-extension ms-dotnettools.csdevkit
code --install-extension github.copilot-chat
code --install-extension humao.rest-client
code --install-extension qwtel.sqlite-viewer
code --install-extension bierner.markdown-mermaid
code --install-extension github.vscode-github-actions
code --install-extension github.vscode-pull-request-github
code --install-extension ms-azuretools.vscode-containers
```

| Eklenti | Ne için |
| --- | --- |
| C# Dev Kit | C# dil desteği, çözüm görünümü, test paneli |
| GitHub Copilot Chat | Copilot: kod önerileri, sohbet, Agent modu |
| REST Client | `Bank.Api.http` dosyasından HTTP isteği göndermek |
| SQLite Viewer | `erdagibank.db` veritabanına bakmak |
| Markdown Preview Mermaid Support | Markdown içindeki Mermaid diyagramlarını görmek (Gün 2) |
| GitHub Actions | Workflow dosyaları (Gün 3) |
| GitHub Pull Requests | Pull request'leri VS Code'dan görmek (Gün 3) |
| Container Tools | Dockerfile desteği (Gün 3, isteğe bağlı) |

**Gün 2 için ek eklenti:** VS Code'da Extensions (Ctrl+Shift+X) → **"GitHub Copilot upgrade"** aratın → Install. Bu eklenti sohbetteki ajan seçiciye **Upgrade** ajanını ekler.

Depo açıldığında VS Code önerilen eklentileri de sağ altta hatırlatır (`.vscode/extensions.json`).

### Copilot'ta oturum açma ve sohbet ayarları

1. VS Code'u açın. Sağ alttaki Copilot simgesine ya da sol alttaki Accounts simgesine tıklayıp **GitHub ile oturum açın**.
2. `Ctrl+Alt+I` ile **Chat** panelini açın ve "merhaba" yazın. Cevap geliyorsa Copilot çalışıyor.
3. Chat panelinde mesaj kutusunun alt satırına bakın:
   - **Oturum hedefi:** `Local` olsun. "Copilot" yazıyorsa üzerine tıklayıp **Local**'i seçin. Prompt dosyaları (`/yeni-endpoint` gibi), Plan modu ve depodaki özel ajanlar yalnızca Local oturumda görünür.
   - **Mod:** `Agent` olsun. Ask ve Plan dosya değiştirmez, komut çalıştırmaz.
4. Mesaj kutusunun yanındaki **araç simgesine** (Configure Tools) tıklayın: terminal ve dosya düzenleme araçları işaretli olsun.

> Ajan "dosyalara erişemiyorum" ya da "komut çalıştıramıyorum" derse bu dört ayardan biri eksiktir. Bir de klasörün **Restricted Mode**'da açılmadığından emin olun (VS Code'un en altında bu yazı görünüyorsa tıklayıp **Trust** deyin).

## 5. uv ve Spec Kit

Spec Kit, Gün 1 · Modül 3'te ve Gün 3 atölyesinde kullandığımız **spec-odaklı geliştirme** aracı. Komut satırı aracının adı `specify`. Python ile yazılmış; Python'u ayrıca kurmanıza gerek yok, **uv** gerekeni kendisi indirir.

```powershell
winget install --id=astral-sh.uv -e
```

Kurulumdan sonra **VS Code'u ve bütün terminalleri kapatıp yeniden açın** (yeni PATH ancak böyle görünür). Sonra:

```powershell
uv --version
uv tool install specify-cli==1.2.4
specify --version
```

`specify --version` sonucu `1.2.4` olmalı. Eğitim bu sürümle hazırlandı. Spec Kit çok hızlı gelişiyor; farklı bir sürümde komut adları ya da dosya yapısı değişebilir.

**Sorun giderme**

| Belirti | Çözüm |
| --- | --- |
| `specify : The term 'specify' is not recognized` | `uv tool update-shell` çalıştırın, VS Code'u kapatıp açın |
| `uv : The term 'uv' is not recognized` | uv kurulumundan sonra VS Code ve terminal yeniden açılmamış |
| `running scripts is disabled on this system` | `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` çalıştırıp `Y` deyin. Spec Kit'in yardımcı betikleri PowerShell betiği |
| Kurmadan bir kez denemek | `uvx --from specify-cli==1.2.4 specify --version` |

## 6. .NET Framework 4.8 Developer Pack (yalnızca Windows, isteğe bağlı)

Gün 2 · Modül 5'in başında 2014'ten kalma EFT sistemini eski haliyle derleyip çalıştırıyoruz. Bunun için [dotnet.microsoft.com/download/dotnet-framework/net48](https://dotnet.microsoft.com/download/dotnet-framework/net48) sayfasından **Developer Pack**'i kurun. Kurmazsanız yalnızca o adımı izlersiniz; .NET 10'a taşınmış hali (`yedek/d2-m5-son`) her platformda çalışır.

## 7. Docker Desktop (isteğe bağlı)

Gün 3 · Modül 7'de Dockerfile'ı yerelde denemek isterseniz [docker.com](https://www.docker.com/products/docker-desktop/) adresinden kurun. Gerekli değil; aynı image GitHub Actions'ta da derleniyor.

## 8. Depoyu indirme

Depoyu **senkronize edilmeyen** bir klasöre indirin. Google Drive, OneDrive, Dropbox gibi klasörlerde senkronizasyon `.git` içindeki dosyaları kilitleyip git komutlarını bozabiliyor (`index.lock: File exists` hatası).

```powershell
mkdir C:\egitim
cd C:\egitim
git clone https://github.com/erturkerdagi/tbb-copilot-egitimi.git
cd tbb-copilot-egitimi
git fetch --all
```

**Gün 3'ü takip edecekseniz:** pull request, issue ve CI adımları için deponun **kendi hesabınızdaki kopyası** gerekir. GitHub'da depo sayfasında **Fork** → "Copy the main branch only" kutusunu **boş bırakın** (bütün branch'ler gelsin) → Create fork. Sonra kendi kopyanızı indirin:

```powershell
git clone https://github.com/<kullanici-adiniz>/tbb-copilot-egitimi.git
```

Fork'ta **Actions** sekmesine girip "I understand my workflows, go ahead and enable them" düğmesine tıklayın; yoksa CI çalışmaz. **Issues** sekmesi kapalıysa Settings → General → Features → Issues kutusunu işaretleyin.

## 9. Son kontrol

Depo klasöründe bir terminal açın ve sırayla çalıştırın:

```powershell
git --version
dotnet --list-sdks
specify --version
dotnet build ErdagiBank.slnx
dotnet test ErdagiBank.slnx
```

| Komut | Beklenen sonuç |
| --- | --- |
| `git --version` | `git version 2.x` |
| `dotnet --list-sdks` | `10.0.x` ile başlayan bir satır |
| `specify --version` | `1.2.4` |
| `dotnet build` | `Build succeeded` (ilk derlemede NuGet paketleri iner; birkaç dakika sürebilir) |
| `dotnet test` | `Passed!  - Failed: 0` |

Sonra uygulamayı bir kez çalıştırın:

```powershell
dotnet run --project src/Bank.Api
```

`Now listening on: http://localhost:5080` satırını görünce VS Code'da `src/Bank.Api/Bank.Api.http` dosyasını açın ve ilk isteğin üstündeki **Send Request**'e tıklayın. `200 OK` görüyorsanız hazırsınız. Terminalde `Ctrl+C` ile uygulamayı durdurun.

> Derleme sırasında sarı **NU1903** uyarısı görebilirsiniz: kullanılan bir NuGet paketinin bilinen bir güvenlik açığı olduğunu söyler. Derlemeyi engellemez. Gün 2 · Modül 6'da bağımlılık riskini konuşurken bu uyarıya döneceğiz.

## macOS ve Linux

- Git, .NET 10 SDK ve VS Code'u kendi paket yöneticinizle (`brew`, `apt`) ya da resmi sitelerinden kurun.
- uv: `curl -LsSf https://astral.sh/uv/install.sh | sh`, sonra `uv tool install specify-cli==1.2.4`.
- Spec Kit'i kurarken demolarda kullanılan `--script ps` yerine `--script sh` kullanın: `specify init --here --integration copilot --script sh --force --non-interactive`.
- 2014 EFT sisteminin eski hali (.NET Framework 4.8) yalnızca Windows'ta derlenir. `yedek/d2-m5-son` ve sonrasındaki .NET 10 hali her platformda çalışır.
- Demolardaki `\` ile yazılmış yolları `/` olarak yazın.
