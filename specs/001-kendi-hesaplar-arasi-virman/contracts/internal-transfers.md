# Sözleşme: Kendi hesaplar arası virman

## İstek

`POST /customers/{customerId}/internal-transfers`

```json
{
  "fromAccountId": 1,
  "toAccountId": 2,
  "amount": 5000,
  "description": "Birikime aktarım"
}
```

## Yanıtlar

| Durum | Ne zaman | `title` |
| --- | --- | --- |
| 201 Created | Başarılı | — (gövde: `TransferResponse`, `kind = "Internal"`) |
| 400 Bad Request | Tutar ≤ 0 veya aynı hesap | `internal_transfer.invalid_amount`, `internal_transfer.same_account` |
| 404 Not Found | Hesap yok veya müşteriye ait değil | `internal_transfer.account_not_found` |
| 422 Unprocessable Entity | Bakiye yetersiz veya para birimi farklı | `internal_transfer.insufficient_balance`, `internal_transfer.currency_mismatch` |
