---
description: Seçili kodu veya değişiklikleri bankacılık güvenlik kurallarına göre inceler
agent: guvenlik-inceleyici
---
Şu kapsamı güvenlik açısından incele: ${input:kapsam:Örn. #changes veya bir dosya}

Özellikle kontrol et:
- SQL injection ve parametresiz sorgular
- Yetkilendirme eksikleri (kullanıcının başkasının hesabına erişebilmesi)
- Loglarda veya yanıtlarda kişisel veri (ad, TCKN, IBAN)
- Koda gömülü şifre, anahtar veya bağlantı bilgisi
- Para hesaplarında `double`/`float` kullanımı

Bulguları tablo halinde ver: Önem (Kritik/Yüksek/Orta/Düşük), Dosya:Satır, Sorun, Önerilen düzeltme.
