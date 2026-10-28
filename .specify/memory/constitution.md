# Erdağı Bank Hesap API Anayasası

## Temel İlkeler

### I. Önce Test (Değiştirilemez)
Her iş kuralı, kodundan önce testle tanımlanır. Testler önce kırmızı görülür, sonra kod yazılır.
Var olan bir testin beklentisi, iş birimi onayı olmadan değiştirilemez.

### II. Kişisel Verinin Korunması
Müşteri adı, TCKN ve tam IBAN; loglara, hata mesajlarına ve API hata yanıtlarına yazılmaz.
Log kayıtlarında yalnızca teknik kimlikler (hesap Id, transfer Id) kullanılır.

### III. Parasal Doğruluk
Tüm tutarlar `decimal` ile tutulur ve hesaplanır. `double` ve `float` para için yasaktır.
Bakiye yalnızca `Account.Deposit` ve `Account.Withdraw` üzerinden değişir.

### IV. Öngörülebilir Hata Yönetimi
İş kuralı ihlalleri exception ile değil `Result<T>` ile döner.
HTTP katmanında hatalar RFC 7807 ProblemDetails formatındadır; hata kodları `alan.hata_adi` biçimindedir.

### V. Sadelik
Yeni bir NuGet paketi, yeni bir proje veya yeni bir mimari katman ancak gerekçesi plan dokümanında yazılıysa eklenir.
Var olan desenler (servis + depo + endpoint) yeniden kullanılır.

## Güvenlik ve Uyum Gereksinimleri

- Bir müşteri yalnızca kendi hesapları üzerinde işlem yapabilir; başkasına ait bir hesabın varlığı hata mesajıyla ele verilmez.
- SQL yalnızca EF Core LINQ veya parametreli sorgularla çalıştırılır.
- Şifre, anahtar ve bağlantı bilgisi kaynak koda yazılmaz.

## Geliştirme Akışı

- Her özellik `specs/` altında bir spesifikasyonla başlar: spec → plan → tasks → implement.
- Her değişiklik `dotnet build` ve `dotnet test` başarılıyken PR'a gider.
- Yapay zekâ tarafından üretilen kod, insan incelemesi olmadan `main`'e birleştirilmez.

## Yönetişim

Bu anayasa diğer tüm geliştirme kurallarının üzerindedir. Değişiklik, teknik lider onayı ve sürüm artırımı gerektirir.
Her PR incelemesinde anayasaya uygunluk kontrol edilir.

**Version**: 1.0.0 | **Ratified**: 2026-11-03 | **Last Amended**: 2026-11-03
