---
applyTo: "tests/**/*.cs"
---
# Test yazım kuralları

- Test adı biçimi: `Metot_Durum_BeklenenSonuç` (ör. `TransferAsync_YetersizBakiye_HataDoner`).
- Her test tek bir davranışı doğrular; Arrange / Act / Assert bölümleri boş satırla ayrılır.
- Sınır değerleri mutlaka test et: sıfır, negatif, limite tam eşit, limit + 0,01.
- Sahte bağımlılıklar için `Fakes/` klasöründeki sınıfları kullan; mock kütüphanesi ekleme.
- Zamana bağlı testlerde `FixedTimeProvider` kullan; `DateTime.Now` kullanma.
- Var olan testlerin beklenen değerlerini, kullanıcı açıkça istemedikçe değiştirme.
