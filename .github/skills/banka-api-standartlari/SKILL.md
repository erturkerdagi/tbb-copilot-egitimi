---
name: banka-api-standartlari
description: Erdağı Bank Hesap API'sine yeni bir REST endpoint, servis metodu veya API sözleşmesi eklenirken kurumun API ve hata yanıtı standartlarını uygular.
---
# Erdağı Bank API standartları

Bu skill, Hesap API'sine yeni bir uç eklenirken kullanılır.

## Uygulama adımları
1. Endpoint yapısı için [endpoint-sablonu.md](endpoint-sablonu.md) dosyasındaki şablonu kullan.
2. Hata yanıtlarını [hata-yaniti-formati.md](hata-yaniti-formati.md) dosyasındaki kod ve HTTP durum eşleştirmesine göre tasarla.
3. URL'ler çoğul isim ve küçük harf kullanır: `/accounts/{id}/deposits`.
4. Tutar alanları `decimal`, para birimi alanı ISO 4217 kodu (`TRY`, `EUR`) olur.
5. Her yeni hata kodu `alan.hata_adi` biçimindedir: `deposit.invalid_amount`.

## Kontrol listesi
- [ ] İstek/yanıt tipleri `Contracts/` altında `record`
- [ ] İş kuralı serviste, endpoint'te değil
- [ ] Hatalar `*Errors.cs` içinde ve doğru `ErrorType` ile
- [ ] POST için `201 Created` + `Location`
- [ ] Her kural için test
- [ ] `Bank.Api.http` dosyasına örnek istek
