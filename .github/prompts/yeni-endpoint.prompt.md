---
description: Kurum standardına uygun yeni bir REST endpoint'i ve testlerini ekler
agent: agent
---
Erdağı Bank Hesap API'sine şu endpoint'i ekle: ${input:endpoint:Örn. Hesaba para yatırma}

Adımlar:
1. İstek ve yanıt tiplerini `Contracts/` altına `record` olarak ekle.
2. İş kurallarını ilgili servise yaz; hataları `*Errors.cs` içinde tanımla ve `Result<T>` döndür.
3. Endpoint'i ilgili `Map*Endpoints` metoduna ekle; yapı olarak #file:../../src/Bank.Api/Endpoints/TransferEndpoints.cs dosyasına uy.
4. Başarılı senaryo ve her iş kuralı için xUnit testi yaz.
5. `Bank.Api.http` dosyasına örnek istek ekle.
6. `dotnet build ErdagiBank.slnx` ve `dotnet test ErdagiBank.slnx` çalıştır, sonucu özetle.

Kod yazmadan önce kısa bir plan göster.
