# Erdağı Bank — Hesap API'si için Copilot talimatları

## Proje
- Erdağı Bank'ın mobil uygulamasına hizmet veren hesap ve transfer API'si.
- .NET 10, ASP.NET Core Minimal API, EF Core 10 (SQLite), xUnit.
- Klasörler: `Domain/` varlıklar, `Services/` iş kuralları, `Data/` EF Core, `Endpoints/` HTTP uçları, `Contracts/` istek/yanıt tipleri.

## Kodlama kuralları
- İş kuralı hatalarında exception fırlatma; `Result<T>` döndür. Hataları `Services/*Errors.cs` içinde `Error` olarak tanımla.
- Para tutarları her zaman `decimal`. `double` veya `float` kullanma.
- Tüm async metotlar `CancellationToken` alır.
- Endpoint'ler iş mantığı içermez; servisi çağırır ve `ToHttpResult()` ile yanıt döner.
- Hata yanıtları ProblemDetails (RFC 7807) formatındadır.
- "Şimdi"yi `DateTime.Now` ile değil, enjekte edilen `TimeProvider` ile al.
- Yeni NuGet paketi eklemeden önce kullanıcıya sor.

## Güvenlik ve kişisel veri
- Loglara, hata mesajlarına ve yanıtlara müşteri adı, TCKN veya tam IBAN yazma. Log'da yalnızca Id kullan.
- SQL'i string birleştirerek oluşturma; EF Core LINQ veya parametreli sorgu kullan.
- Şifre, anahtar ve bağlantı bilgisini koda yazma.

## Testler
- Her yeni iş kuralı için `tests/Bank.Api.Tests` altında xUnit testi yaz.
- Testlerde `Fakes/` altındaki bellek içi depoları ve `FixedTimeProvider`'ı kullan.

## Doğrulama
- Değişiklikten sonra `dotnet build ErdagiBank.slnx` ve `dotnet test ErdagiBank.slnx` çalıştır ve sonucu özetle.
