# Rehber

Bu klasör, eğitimdeki demoları **kendi başınıza** yapabilmeniz için hazırlandı. Eğitimde canlı yaptığımız her demo burada aynı sırayla, adım adım duruyor.

| Dosya | İçerik |
| --- | --- |
| [00-HAZIRLIK.md](00-HAZIRLIK.md) | Eğitimden önce yapılacak kurulumlar ve kontrol listesi |
| [GUN1-DEMOLAR.md](GUN1-DEMOLAR.md) | Gün 1: Copilot'un çalışma biçimi, kurumsal özelleştirme, Spec Kit |
| [GUN2-DEMOLAR.md](GUN2-DEMOLAR.md) | Gün 2: Test odaklı geliştirme, legacy modernizasyonu, güvenlik incelemesi |
| [GUN3-DEMOLAR.md](GUN3-DEMOLAR.md) | Gün 3: CI/CD, yönetişim, uçtan uca atölye |

## Nasıl kullanılır?

- Her demonun başında **Amaç** ve **Başlangıç noktası** (hangi branch'ten başlandığı) yazar. O branch'e geçerek demoyu bağımsız olarak yapabilirsiniz.
- Adımların altındaki kutular:
  - 💡 **Neden?** Bu adımı neden yaptığımız.
  - ✅ **Onay (Allow) istenirse** Copilot bir komut çalıştırmak istediğinde neye izin verdiğimiz.
  - 🔎 **Sonucu nasıl okuruz?** Ekranda çıkan sonucun anlamı.
  - ⚠️ **Sorun olursa** Takıldığınızda ne yapacağınız.
- Kopyalanacak metinler (prompt'lar ve terminal komutları) aynı sırayla [`docs/PROMPTLAR-GUN1.md`](../docs/PROMPTLAR-GUN1.md), [`GUN2`](../docs/PROMPTLAR-GUN2.md) ve [`GUN3`](../docs/PROMPTLAR-GUN3.md) dosyalarında da var.

## Unutmayın

- Copilot olasılıksal çalışır: aynı prompt'a her seferinde biraz farklı cevap verebilir. Sonucunuz rehberdekinden farklıysa bu normal. Her demonun sonunda bitmiş hali bir **yedek branch**'te durur; karşılaştırmak için oraya geçebilirsiniz.
- Değişiklikleri geri almanın en güvenli yolu git:

  ```powershell
  git restore --staged --worktree .   # değişen dosyaları eski haline getirir
  git clean -fd                       # yeni eklenen dosyaları siler
  ```

- Copilot bir komut çalıştırmak için onay istediğinde (Allow), komutu okuyun. "Always allow" yerine her seferinde tek tek izin vermek, ajanın ne yaptığını görmenin en iyi yolu.
