---
applyTo: "src/**/Endpoints/**/*.cs"
---
# Endpoint yazım kuralları

- Endpoint'ler ilgili `Map*Endpoints` uzantı metodunda, `MapGroup` ile gruplanır ve `WithTags` alır.
- Her endpoint `WithName` ve Türkçe `WithSummary` içerir.
- Endpoint gövdesinde iş kuralı yazılmaz; servis çağrılır, sonuç `ToHttpResult()` ile döner.
- Kaynak oluşturan POST istekleri `201 Created` ve `Location` başlığı döner.
- İstek ve yanıt tipleri `Contracts/` altında `record` olarak tanımlanır; domain varlıkları doğrudan dışarı açılmaz.
