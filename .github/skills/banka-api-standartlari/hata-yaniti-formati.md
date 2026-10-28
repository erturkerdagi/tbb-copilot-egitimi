# Hata yanıtı formatı (RFC 7807 ProblemDetails)

| ErrorType | HTTP durum kodu | Ne zaman |
| --- | --- | --- |
| `Validation` | 400 Bad Request | İstek biçimi veya değeri geçersiz (negatif tutar, boş alan) |
| `NotFound` | 404 Not Found | Hesap, müşteri veya kayıt bulunamadı |
| `BusinessRule` | 422 Unprocessable Entity | İstek geçerli ama iş kuralı engelliyor (yetersiz bakiye, limit) |

Örnek yanıt:

```json
{
  "type": "https://tools.ietf.org/html/rfc4918#section-11.2",
  "title": "transfer.insufficient_balance",
  "status": 422,
  "detail": "Hesap bakiyesi yetersiz."
}
```

Kurallar:
- `title` alanı hata kodudur; istemci uygulamalar buna göre davranır.
- `detail` alanı Türkçe ve kullanıcıya gösterilebilir olmalıdır.
- Hata mesajlarında ad, TCKN, IBAN veya bakiye tutarı yer almaz.
